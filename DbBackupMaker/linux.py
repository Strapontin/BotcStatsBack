import os
import platform
from datetime import datetime
import subprocess

# If file doesn't exists
if not os.path.exists("./secrets"):
    print(f"No secrets file found at {os.path.abspath(os.path.curdir)}")
    input()
    exit()

mySecrets = {}
with open("./secrets", "r") as file:
    for line in file:
        name, var = line.partition("=")[::2]
        mySecrets[name.strip()] = var.strip()


timestamp = datetime.now().strftime("%Y_%m_%d")
backup_file = f"{mySecrets['BACKUP_DIRECTORY']}botcstat_{timestamp}.backup"

print("\n\n")

if os.path.exists(backup_file):
    print(f"File already exists at {backup_file}. Exiting")
    input()
    exit()

try:
    print("Backuping...")
    print("\n")
    
    subprocess.run(f"export PGPASSWORD={mySecrets['PASSWORD']}\n" +
                   f"pg_dump " +
                   f"-h {mySecrets['HOST']} " +
                   f"-U {mySecrets['USER']} " +
                   f"-d {mySecrets['DATABASE']} " + 
                   f"-Fc -f {backup_file}", shell=True, check=True)
    
    print(f"\nBackup completed successfully. Backup file is {backup_file}")
except subprocess.CalledProcessError as ex:
    print("\nError during backup:", ex)

if os.path.getsize(backup_file) == 0:
    os.remove(backup_file)
input()
