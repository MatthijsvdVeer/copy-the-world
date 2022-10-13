# Introduction
This repository contains a reference implementation for Azure Digital Twins. It focuses on splitting streaming ingress (devices and others) from graph-led ingress (e.g. weather data). Changes in the graph are subscribed to by functions with a single responsibility, rather than adding logic to your ingress layer. I created this reference to accompany my **Copy The World With Azure Digital Twins** presentation. You can find the [slides here][1], feel free to check out the presentation abstract on [Sessionize][2].

## High-level overview
![Architecture][3]

## Application overview
![Full application overview][4]

## The resulting graph
![The graph][5]


# I want to check this out!
You can deploy this application to your Azure subscription. To do so, you'll need to add a couple of repository secrets.

## Secrets:
### AZURE_RESOURCE_GROUP_NAME
The existing resource group name (create this yourself)

### AZURE_CREDENTIALS
The contents of this secret can be generated with the following command:
```
az ad sp create-for-rbac --name CTWGitHubActions --role owner --scopes /subscriptions/{subscription-id}/resourceGroups/{resource-group-name} --sdk-auth
```

### AZURE_CLIENT_ID
The ID of the application generated with the command above

### AZURE_SUBSCRIPTION_ID
Your Azure subscription ID

### Your Azure tenant ID
AZURE_TENANT_ID

## Workflows
The [Deployment workflow][6] will deploy all the infrastructure, as well as the code and configuration.
Afterward, you can run the [🔥 Reset Demo Content][7] workflow to upload all the twins and relationships, as well as the mapping configuration. Running it again will overwrite the entire graph.

*Note: I like you, but please check the workflow to make sure you agree with what it's doing*

## Considerations
Adding an Event Hub namespace and an S1 IoT Hub means there are costs involved even if you're not using the application. You can change the hub to be Free tier. You could also swap out the event hub for something else, but I do recommend Event Hubs for production workloads. All the other components are pay-per-use.

[1]: /presentation/Copy%20The%20World%20With%20Azure%20Digital%20Twins.pdf
[2]: https://sessionize.com/app/speaker/session/344004
[3]: /presentation/images/high-level-arch.png
[4]: /presentation/images/application-overview.png
[5]: /presentation/images/graph.png
[6]: /.github/workflows/deploy.yml
[7]: /.github/workflows/reset-demo.yml