WebForms FileManager mini-module

1) Copy the 'FileManager' folder into your ASP.NET WebForms (.NET Framework 4.8) web project.

2) Storage location:
      ~/FileManager/Files

3) Permissions:
      Give IIS App Pool identity Modify rights on that folder.

4) Namespace:
      Generated code uses namespace 'YourApp.FileManager'
      - Option A: Change 'YourApp' in the .cs files to your real root namespace.
      - Option B: Keep code as-is and ONLY change Inherits= in FileManager.aspx accordingly.

5) Browse:
      /FileManager/FileManager.aspx
