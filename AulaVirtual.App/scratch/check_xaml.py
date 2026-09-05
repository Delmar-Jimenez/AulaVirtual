import os

views_dir = r"C:\Users\delma\OneDrive\Documents\AulaVirtual\AulaVirtual.App\Views"
malformed = []

for filename in os.listdir(views_dir):
    if filename.endswith(".xaml"):
        filepath = os.path.join(views_dir, filename)
        with open(filepath, "r", encoding="utf-8") as f:
            content = f.read()
            if "</ContentPage>" not in content:
                malformed.append(filename)

print("Malformed files:", malformed)
