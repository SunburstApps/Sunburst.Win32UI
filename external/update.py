#!/usr/bin/env python2.7

from __future__ import print_function
import sys
import os
import json
import subprocess

def popen_exec(args, cwd=None, ignore_exitcode=False):
    process = subprocess.Popen(args, cwd=cwd)
    process.wait()

    if process.returncode != 0 and not ignore_exitcode:
        raise OSError, "{} failed with code {}".format(args, process.returncode)

all_deps = {}
processed_deps = []
new_dep_count = 0

def collect_dependencies(json_path):
    json_data = None
    with open(json_path, 'r') as fh:
        json_data = json.load(fh)

    for (repo_name, repo_remote) in json_data.items():
        if not repo_name in processed_deps:
            all_deps[repo_name] = repo_remote

            global new_dep_count
            new_dep_count += 1

def process_dependencies(working_directory):
    for (repo_name, repo_remote) in all_deps.items():
        if repo_name in processed_deps: continue

        repo_dir = os.path.join(working_directory, repo_name)
        if not os.path.exists(repo_dir):
            popen_exec(['git', 'clone', repo_remote, repo_name], cwd=working_directory)
        else:
            popen_exec(['git', 'pull'], cwd=repo_dir)

        child_json_path = os.path.join(repo_dir, 'external', 'dependencies.json')
        if os.path.exists(child_json_path):
            collect_dependencies(child_json_path)
            with open(child_json_path + '.ignore', 'w') as fh:
                print('Do not run update.py in this directory - it will only confuse things', file=fh)

        processed_deps.append(repo_name)

working_directory = os.path.dirname(__file__)
json_path = os.path.join(working_directory, 'dependencies.json')

if os.path.exists(json_path + '.ignore'):
    print('Do not run update.py in this directory - it will only confuse things', file=sys.stderr)
    print('Run update.py in the top-level project directory instead', file=sys.stderr)
    exit(1)

collect_dependencies(json_path)

while new_dep_count != 0:
    new_dep_count = 0
    process_dependencies(working_directory)
