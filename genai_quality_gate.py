import os
import sys
import requests
import json
import xml.etree.ElementTree as ET

def parse_cobertura_issues(xml_path):
    issues = []
    if not os.path.exists(xml_path):
        return issues
    tree = ET.parse(xml_path)
    root = tree.getroot()
    for cls in root.findall('.//class'):
        filename = cls.attrib.get('filename')
        for line in cls.findall('lines/line'):
            if line.attrib.get('hits') == '0':
                issues.append(f"{filename}:{line.attrib.get('number')}: Not covered by tests.")
    return issues

def build_prompt(metrics, code_smells):
    code_smell_list = '\n'.join(code_smells) if code_smells else 'None.'
    return (
        "You are an AI code reviewer. Given the following code metrics and issues, "
        "should this PR pass the quality gate? Respond with 'PASS' or 'FAIL' and explain your reasoning. "
        "If you fail, reference the exact file and line numbers in your explanation.\n"
        f"Coverage: {metrics.get('coverage', 'N/A')}\n"
        f"Code Smells:\n{code_smell_list}\n"
        f"Diff: {metrics.get('diff', 'N/A')}\n"
    )

def call_gemini(prompt):
    api_key = os.getenv('GEMINI_API_KEY', 'AIzaSyDR9MkXL4aVgffFBU0bdpN1iPy6293s-BI')
    endpoint = f"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={api_key}"
    body = {
        "contents": [
            {"parts": [{"text": prompt}]}
        ]
    }
    resp = requests.post(endpoint, json=body)
    resp.raise_for_status()
    return resp.json()

def main():
    # Example: parse code smells from coverage (or replace with SonarCloud/linter output)
    cobertura_path = os.getenv('COVERAGE_FILE', 'Calculator.Tests/TestResults/coverage.cobertura.xml')
    code_smells = parse_cobertura_issues(cobertura_path)
    metrics = {
        'coverage': '80%',
        'diff': 'Minor refactor.'
    }
    prompt = build_prompt(metrics, code_smells)
    print(f"Prompt to Gemini:\n{prompt}\n")
    response = call_gemini(prompt)
    print(f"Gemini raw response:\n{json.dumps(response, indent=2)}\n")
    text = response['candidates'][0]['content']['parts'][0]['text'].strip().upper()
    if 'PASS' in text:
        print(f"\nGenAI Quality Gate: PASS\nExplanation: {text}")
        sys.exit(0)
    else:
        print(f"\nGenAI Quality Gate: FAIL\nExplanation: {text}")
        sys.exit(1)

if __name__ == "__main__":
    main()
