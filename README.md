# CLDV6212 POE Part 1 - CoffeeNChill

## Team Members:

- Muhammad Naveed
- Luthando Mtshali
- Nathan Beaumont
- Coherence Mlambo

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

docker run --name coffeenchill-azurite 
  -p 10000:10000 
  -p 10001:10001 
  -p 10002:10002 
  mcr.microsoft.com/azure-storage/azurite

## Commits:

- Installed Azure Storage Dependencies and Set Up Project Structure
- Created MenuItem Model to Store Menu Item Details
- Created TableStorageService to Manage MenuItem CRUD Operations
- Created CreateMenuItem to Validate and Create Menu Item
- Created GetAllMenuItems to Retrieve All The Menu Items

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

### Azure Blob Storage Service

The BlobStorageService handles communication between the CoffeeNChill Azure Functions 
and Azure Blob Storage. The BlobStorageService provides functionality for Uploading 
staff documents, retrieving a list of stored documents. downloading stored documents,
and checking whether a requested document exists. For local development, the service 
connects to the Azurite storage emulator and uses the 'staff-docs' Blob container.

### Upload Staff Document

The UploadStaffDocument function uploads a staff document using 'multipart/form-data'
and is stored inside the 'staff-docs' Blob Storage container.


### List Staff Documents

This ListStaffDocuments function retrieves the stored documents from the 'staff-docs' 
Blob Storage container.

### Download Staff Document

The DownloadStaffDocument function downloads the staff documents from the 'staff-docs'
Blob Storage container.

### Commits:

- Created BlobStorageService for Staff Documents
- Created UploadStaffDocument to Upload All The Staff Documents
- Created ListStaffDocuments to List The Stored Staff Documents
- Created DownloadStaffDocument to Download the Staff Documents
- Updated UploadStaffDocument, ListStaffDocuments, and DownloadStaffDocument


