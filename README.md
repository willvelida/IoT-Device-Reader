# IoT-Device-Reader

This is a sample application showcasing the use of Azure Cosmos DB (SQL API) within a Azure Function application. 

## Functions

This sample uses a hypothetical IoT device reader and implements the following:

- CosmosDB Trigger Function (*MaterializeReadings*). This function uses the Cosmos DB Change Feed to listen to activity in *DeviceReadings* container and then persists those changes into another Cosmos DB container (*DeviceCore*).
- Service Bus Trigger Function (*ProcessReadings*). This function reads incoming messages from a Service Bus Queue and persists each message as a item in a Cosmos DB container *DeviceReadings*. This sample uses Dependency Injection to connect to the container in Cosmos DB.
- HTTP Trigger (*GenerateReadings*). This function generates sample IoT device reading via a POST requests and sends each reading as a message to a Service Bus queue.
- Various HTTP Triggers (*DeviceBindingsAPI*). This showcases the use of both input and output Cosmos DB bindings to retrieve and create device readings.

## Working with the sample

This sample requries the following infrastructure provisioned in Azure:

- Azure Cosmos DB account (SQL API).
- One Database ('*DeviceDB*') and Two Collections ('*DeviceReadings*' and '*DeviceCore*'). The lease container required for the Change Feed Function is created if the container does not exist.
- Service Bus namespace (Standard Pricing)
- Service Bus queue called '*devicereadings*'.

## Contributing

If you want to suggest changes, implement a sample or have any other feedback, I welcome contributions!

The following tools are recommended to develop and work with the sample.

- Visual Studio or Visual Studio Code.
- Postman (Testing HTTP Endpoints)