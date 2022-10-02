# Introduction
This repository contains all the code that accompanies my Copy The World With Azure Digital Twins presentation.

# I want to check this out!
You can deploy this application to your Azure subscription. To do so, you'll need to add a couple of repository variables.

AZURE_CREDENTIALS
The contents of this secret can be generated with the following command:
```
az ad sp create-for-rbac --name CTWGitHubActions --role owner --scopes /subscriptions/{subscription-id}/resourceGroups/{resource-group-name} --sdk-auth
```