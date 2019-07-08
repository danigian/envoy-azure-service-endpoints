## Storage Account: Create Blob Container, Put text file and Get it through Envoy Proxy

This sample tries to create a blob container, put a text file in it and then get it to prove that it worked.

### How to make it work

0.  The Storage Account needs to be locked down to the VNet in which your AKS cluster is deployed (info [here](https://docs.microsoft.com/en-us/azure/storage/common/storage-network-security))

1.  Set the following environment variables

    `proxyUrl`: the Envoy Proxy URL (i.e.: "https://storage.envoy.mydomain.com/")

    `storageName`: the Storage Account name

    `storageKey`: the Storage Account primary key
    
    `containerName`: the Storage Container name that it's going to be created

2.  Run!