import os
import re

views_dir = r"C:\Users\delma\OneDrive\Documents\AulaVirtual\AulaVirtual.App\Views"
xaml_classes = {}
cs_namespaces = {}

for f in os.listdir(views_dir):
    if f.endswith('.xaml'):
        content = open(os.path.join(views_dir, f), encoding='utf-8').read()
        match = re.search(r'x:Class="([^"]+)"', content)
        if match:
            xaml_classes[f] = match.group(1)

for f in os.listdir(views_dir):
    if f.endswith('.xaml.cs'):
        content = open(os.path.join(views_dir, f), encoding='utf-8').read()
        match_ns = re.search(r'namespace\s+([^;{\s]+)', content)
        match_cls = re.search(r'class\s+([^\s:]+)', content)
        if match_ns and match_cls:
            cs_namespaces[f] = f"{match_ns.group(1)}.{match_cls.group(1)}"

for f, cls in xaml_classes.items():
    cs_file = f + '.cs'
    cs_cls = cs_namespaces.get(cs_file)
    if cls != cs_cls:
        print(f"MISMATCH: {f} x:Class={cls} but C#={cs_cls}")
