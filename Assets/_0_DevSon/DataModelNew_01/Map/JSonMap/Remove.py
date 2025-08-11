import os


filepath = "D:\Hole Reedbeen\Assets\TextAsset_2"
for filename in os.listdir(filepath):

    print(filename)
    if filename.endswith(".py"):
        continue
    if  filename.startswith("MapData") and  filename.endswith(".json") :
        print("Removing:", filename)
        continue
    else:
        file_path = os.path.join(filepath, filename)
       
        os.remove(file_path)