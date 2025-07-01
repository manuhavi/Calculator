import os
import sys
import requests
import json

def build_prompt(metrics):
    return (
        "You are an AI code reviewer. Given the following code metrics and changes, "
        "should this PR pass the quality gate? Respond with 'PASS' or 'FAIL' and explain your reasoning.\n"
        f"Coverage: {metrics.get('coverage', 'N/A')}\nCode Smells: {metrics.get('smells', 'N/A')}\nBugs: {metrics.get('bugs', 'N/A')}\nVulnerabilities: {metrics.get('vulns', 'N/A')}\nDiff: {metrics.get('diff', 'N/A')}\n"
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
    # Example: you can parse real metrics here
    metrics = {
        'coverage': '80%',
        'smells': 2,
        'bugs': 0,
        'vulns': 0,
        'diff': 'Minor refactor.'
    }
    prompt = build_prompt(metrics)
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
