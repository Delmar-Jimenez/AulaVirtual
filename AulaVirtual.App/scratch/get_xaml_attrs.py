import os
import re

views_dir = r"C:\Users\delma\OneDrive\Documents\AulaVirtual\AulaVirtual.App\Views"
attributes = set()

pattern = re.compile(r'\b([a-zA-Z]+)="[^"]*"')

for filename in os.listdir(views_dir):
    if filename.endswith(".xaml"):
        filepath = os.path.join(views_dir, filename)
        with open(filepath, "r", encoding="utf-8") as f:
            content = f.read()
            matches = pattern.findall(content)
            for match in matches:
                attributes.add(match)

print(sorted(list(attributes)))
