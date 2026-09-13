# CLDV6212 POE Part 1 - CoffeeNChill

## Team Members:

- Muhammad Naveed
- Luthando Mtshali
- Nathan Beaumont
- Coherence Mlambo

## Docker Hub Images

The Docker images used for the CoffeeNChill application have been published to Docker Hub with the required `v1.0` tags.

- CoffeeNChill Functions: https://hub.docker.com/repository/docker/muhammadnaveed1101/coffeenchill-functions/general 
- Image: `muhammadnaveed1101/coffeenchill-functions:v1.0`

- CoffeeNChill Azurite: https://hub.docker.com/repository/docker/muhammadnaveed1101/coffeenchill-azurite/general 
- Image: `muhammadnaveed1101/coffeenchill-azurite:v1.0`

## Architecture Overview

CoffeeNChill is an Azure Functions application that uses HTTP-triggered functions to provide API functionality for managing menu items and staff documents.

The application uses:

- Azure Table Storage to store and manage menu item information in the 'MenuItems' table
- Azure Blob Storage to store and manage staff PDF documents in the 'staff-docs' container
- Azurite to emulate Azure Storage services locally
- Docker to run Azurite and the CoffeeNChill Azure Functions application in standalone containers
- Postman to test all eight HTTP-triggered API endpoints and verify that the Dockerised application works correctly

## Member 1 - Muhammad Naveed 

## Responsiblity:

I was responsible for developing the Azure Table Storage foundation and 
the initial menu management functionality for the CoffeeNChill system.

## Contributions:

- Installed Azure Storage Dependencies
- Set Up Project Structure 
- Created the MenuItem model
- Created the TableStorageService
- Created the CreateMenuItem function
- Created the GetAllMenuItems function
- Configured the application to communicate with Azurite Table Storage

## Features Implemented:

### MenuItem Model:

The MenuItem model is responsible for storing CoffeeNChill menu information in
Azure Table Storage.

The model contains:

- PartitionKey – stores the menu category
- RowKey – stores the unique menu item ID
- Name – stores the menu item name
- Description – stores the menu item description
- Price – stores the menu item price
- IsAvailable – stores the availability status

### Azure Table Storage Service:

The TableStorageService handles communication between the CoffeeNChill 
Azure Functions and Azure Table Storage. The TableStorageService connects
to the local Azurite storage emulator and uses the MenuItems table to store
menu information.

### Create Menu Item:

The CreateMenuItem function accepts menu item information in JSON format
and stores the new menu item in the MenuItems Azure Table. The CreateMenuItem 
also performs validation before creating the menu item.

### Get All Menu Items:

The GetAllMenuItems function retrieves the menu items stored in the MenuItems table
and returns them to the user.

## Local Azurite Setup:

I used Azurite to emulate Azure Storage during local development.
I started the Azurite Docker container using the following command:

```powershell
docker run --name coffeenchill-azurite `
  -p 10000:10000 `
  -p 10001:10001 `
  -p 10002:10002 `
  mcr.microsoft.com/azure-storage/azurite
```

## Commits:

- Installed Azure Storage Dependencies and Set Up Project Structure
- Created MenuItem Model to Store Menu Item Details
- Created TableStorageService to Manage MenuItem CRUD Operations
- Created CreateMenuItem to Validate and Create Menu Item
- Created GetAllMenuItems to Retrieve All The Menu Items
- Muhammad Naveed README.md

## Member 2 - Luthando Mtshali

## Responsiblity:

I was responsible for completing the remaining MenuItem management
functionality for the CoffeeNChill application.

## Contributions:

- Created the GetMenuItemsByCategory function
- Created the UpdateMenuItem function
- Created the DeleteMenuItem function
- Updated the CreateMenuItem and GetAllMenuItems functions
- Updated the GetMenuItemsByCategory, UpdateMenuItem, and DeleteMenuItem functions

# Features Implemented:

### Get Menu Items By Category:

The GetMenuItemsByCategory function retrieves menu items that belong to a specific
category from the MenuItems Azure Storage Table. The category is stored as the
PartitionKey of each menu item.

### Update Menu Item:

The UpdateMenuItem function allows an existing menu item to be updated. The UpdateMenuItem 
function locates the Menu Item using category as the PartitionKey and Menu Item ID as the RowKey.

### Delete Menu Item:

The DeleteMenuItem function removes an existing menu item from Azure Table Storage.
The menu item is identified using its category and unique menu item ID.

## Commits:

- Created GetMenuItemsByCategory to Retrieve Menu Items By Category
- Created UpdateMenuItem to Update Menu Items
- Created DeleteMenuItem to Delete The Menu Items
- Updated CreateMenuItem and GetAllMenuItems
- Updated GetMenuItemsByCategory, UpdateMenuItem, and DeleteMenuItem
- Luthando Mtshali README.md

## Member 3 - Nathan Beaumont

## Responsiblity:

I was responsible for implementing the staff document management functionality 
for the CoffeeNChill application using Azure Blob Storage.

## Contributions:

- Created the BlobStorageService
- Created UploadStaffDocument function
- Created ListStaffDocuments function
- Created DownloadStaffDocument function
- Updated UploadStaffDocument, ListStaffDocuments, and DownloadStaffDocument 
- Configured the staff-docs Blob Storage container
- Added PDF file type validation and document error handling

## Features Implemented:

### Azure Blob Storage Service:

The BlobStorageService handles communication between the CoffeeNChill Azure Functions 
and Azure Blob Storage. The BlobStorageService provides functionality for Uploading 
staff documents, retrieving a list of stored documents. downloading stored documents,
and checking whether a requested document exists. For local development, the service 
connects to the Azurite storage emulator and uses the 'staff-docs' Blob container.

### Upload Staff Document:

The UploadStaffDocument function uploads a staff document using 'multipart/form-data'
and is stored inside the 'staff-docs' Blob Storage container.


### List Staff Documents:

The ListStaffDocuments function retrieves the stored documents from the 'staff-docs' 
Blob Storage container.

### Download Staff Document:

The DownloadStaffDocument function downloads the staff documents from the 'staff-docs'
Blob Storage container.

## Commits:

- Created BlobStorageService for Staff Documents
- Created UploadStaffDocument to Upload All The Staff Documents
- Created ListStaffDocuments to List The Stored Staff Documents
- Created DownloadStaffDocument to Download the Staff Documents
- Updated UploadStaffDocument, ListStaffDocuments, and DownloadStaffDocument
- Nathan Beaumont README.md

## Member 4 - Coherence Mlambo

## Responsiblity:

I was responsible for setting up the CoffeeNChill Azure Functions application in a Docker 
environment, I configured Docker to work with Azurite for local Azure Table Storage and
Blob Storage, I created and published the necessary Docker images to Docker Hub, and I 
tested the API integration using Postman.

## Contributions:

- Created and configured the Docker setup for the Azure functions application
- Created the multi-stage Dockerfile for the Azure functions application
- Built the CoffeeNChill functions Docker image
- Configured the functions container to communicate with Azurite
- Tagged and published the CoffeeNChill Functions image to Docker Hub
- Tagged and published the Azurite image to Docker Hub
- Created the Postman collection for the CoffeeNChill API
- Tested all 8 required API endpoints using Postman
- Created automated tests in Postman
- Performed final integration testing between Azure Functions, Azurite Table Storage, Blob Storage, Docker, and Postman

## Features Implemented:

### Dockerfile:

A multi-stage Dockerfile was created to containerise the CoffeeNChill Azure Functions application. The Dockerfile uses 
the .NET 10 SDK image to restore, build and publish the CoffeeNChill project. It uses the official Azure Functions .NET
isolated runtime image to run the published application. It copies the published application into the Azure Functions 
runtime container. It configures the Azure Functions script root and console logging and it allows the CoffeeNChill API
to run independently as a standalone Docker container.

### .dockerignore:

A .dockerignore file was created to exclude unnecessary files and folders from the Docker build context. The .dockerignore
file excludes build folders such as 'bin' and 'obj', Visual Studio development files such as '.vs', Git repository files,
and 'local.settings.json'. This helps keep the Docker build context clean and reduces unnecessary files in the final build.

### Azurite Docker Container Setup:

Azurite was configured to run as a standalone Docker container to provide local Azure Storage emulation for the CoffeeNChill
application.

### CoffeeNChill Functions Docker Container:

The CoffeeNChill Azure Functions application was configured to run inside its own standalone Docker container.

### Docker Hub Publishing:

The CoffeeNChill Docker images were tagged and published to Docker Hub using the required 'v1.0' version tag.

The published images are:

- `muhammadnaveed1101/coffeenchill-functions:v1.0`
- `muhammadnaveed1101/coffeenchill-azurite:v1.0`

### Postman Collection:

A Postman collection was created to test all eight HTTP-triggered Azure functions implemented in the CoffeeNChill application.

The Postman collection:

- Is organised into separate 'Menu' and 'Documents' folders
- Uses the `{{baseUrl}}` collection variable instead of hardcoded URLs
- Contains requests for all five Menu API endpoints
- Contains requests for all three Staff Document API endpoints
- Includes the required request bodies and parameters for testing the API
- Was tested against the CoffeeNChill application running inside the Docker container
- Was exported as a JSON file and stored in the project's 'docs' folder

### Automated Postman Tests:

Automated tests were added to the Postman collection to verify that the CoffeeNChill API endpoints return the expected results.

The automated tests include:

- Create Menu Item which checks that a new menu item is created successfully
- Get All Menu Items which checks that all stored menu items are returned successfully
- Get Menu Items by Category which checks that menu items are correctly filtered by their category
- Update Menu Item which checks that the price and availability of a menu item are updated successfully
- Delete Menu Item which checks that a menu item is deleted successfully
- Upload Staff Document which checks that a PDF document is uploaded successfully
- List Staff Documents which checks that all uploaded documents and their information are returned
- Download Staff Document which checks that the requested PDF document is downloaded successfully

## Local Setup Steps:

Before running the CoffeeNChill application locally, ensure the following software is installed:

- Visual Studio
- .NET 10 SDK
- Docker Desktop
- Postman
- Git

Thereafter, to set up the project locally:

1. Clone the CoffeeNChill GitHub repository
2. Open the 'CoffeeNChill' solution in Visual Studio
3. Ensure Docker Desktop is running
4. Ensure the required NuGet packages are restored
5. Start the Azurite Docker container
6. Build the CoffeeNChill Functions Docker image
7. Run the CoffeeNChill Functions container
8. Verify that both Docker containers are running
9. Open Postman and use `http://localhost:7071` as the `{{baseUrl}}`
10. Run the CoffeeNChill Postman collection to test all API endpoints

## Standalone Docker Execution Commands:

1. Run the Azurite Container

   ```powershell
   docker run --name coffeenchill-azurite ` 
     -p 10000:10000 `
     -p 10001:10001 `
     -p 10002:10002 `
     mcr.microsoft.com/azure-storage/azurite
   ```

2. Build the CoffeeNChill Functions Image

   From the project directory containing the 'Dockerfile', run:

  ```powershell
   docker build -t coffeenchill-functions:v1.0 .
  ```

3. Run the CoffeeNChill Functions Container

   ```powershell
   docker run --name coffeenchill-functions`
     -p 7071:80` 
     -e  AzureWebJobsStorage="DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://host.docker.internal:10000/devstoreaccount1;QueueEndpoint=http://host.docker.internal:10001/devstoreaccount1;TableEndpoint=http://host.docker.internal:10002/devstoreaccount1;" `
     -e   StorageConnectionString="DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://host.docker.internal:10000/devstoreaccount1;QueueEndpoint=http://host.docker.internal:10001/devstoreaccount1;TableEndpoint=http://host.docker.internal:10002/devstoreaccount1;" `
coffeenchill-functions:v1.0 ```

4. Verify the Running Containers

   ```powershell
   docker ps
   ```

   Both containers should be running:

- `coffeenchill-azurite`
- `coffeenchill-functions`

5. Access the CoffeeNChill API in Postman

   The Dockerised Azure Functions application is available at:

   URL:

   `http://localhost:7071`

   The Functions container should load all eight required HTTP-triggered Azure Functions.

6. Test the Dockerised Application in Postman
   
   The Postman collection uses:

   URL:

   `{{baseUrl}} = http://localhost:7071`

    For example, the Get All Menu Items endpoint can be tested in Postman using:

   Method: `GET`

   Request URL:

   `{{baseUrl}}/api/menu`

   This sends a request to the CoffeeNChill Functions container running on port `7071` and returns the menu items stored in Azurite Table Storage.

   Thereafter, run the 'CoffeeNChill API' Postman collection to verify that all Menu and Document endpoints work successfully while the application is running inside Docker.

   ## Commits:

   - Created Dockerfile for Azure Functions
   - Created .dockerignore
   - Created Postman Collection with Automated API Tests
   - Updated UploadStaffDocument
   - Coherence Mlambo README.md
   - Docker Hub Images
   - YouTube Link
