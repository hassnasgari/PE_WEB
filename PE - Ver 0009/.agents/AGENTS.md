# Persian & English Mixed Text Formatting Rule

## Language & Direction Rules
1. **RTL Formatting for Persian**: Always wrap Persian text blocks in `<div dir="rtl">...</div>` so that Persian text is rendered right-aligned and right-to-left.
2. **Line Separation for English / Technical Terms**: Never mix English words, technical terms, code snippets, status text, or numbers/metrics (e.g., `npm run dev`, `Weekly Limit`, `Gemini 3.6 Flash`) in the same line with Persian text.
3. **Dedicated English Lines**: Place all English sentences, commands, variables, code snippets, and English UI labels on separate dedicated lines below/above the Persian text block.
