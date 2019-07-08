# Envoy Proxy Configuration

This folder contains the proxy configuration (envoy.yaml), the Dockerfile to build a proper docker image and a Kubernetes YAML file to deploy the newly built image to your cluster (deploy.yaml)

## How to set-up

### Prerequisites

Before starting with Envoy you will need to:

0.  Create Azure KeyVault and/or Azure Storage Account resource.

    Create Azure Kubernetes Service with custom VNet integration

    Create an Azure Container Registry accessible from the AKS Service Principal

1.  Lockdown the newly created resource to the VNet in which your AKS Cluster is ([KeyVault](https://docs.microsoft.com/en-us/azure/key-vault/key-vault-network-security), [Storage Account](https://docs.microsoft.com/en-us/azure/storage/common/storage-network-security))

2.  Create DNS A records in your DNS Zone to make your proxy accessible via a DNS Name 
    
    (*if you still do not have the Internal Load Balancer URL for the Kubernetes Service you can postpone this step*)

3.  Create SSL certificates for your proxy endpoints. These are used by the Envoy Listener to protect User<-->Proxy communications.
    
    The envoy.yaml is configured to accept requests sent to these domains. 
    
    Therefore you need to create certificates for something like:

    keyvault.envoy.mydomain.com

    storage.envoy.mydomain.com

### Create the Envoy docker image

0.  Modify the envoy.yaml file to point to your KeyVault/Storage (find and replace damaggiotemp.blob.core.windows.net and damaggiotemp.vault.azure.net)

1.  docker build -t yourregistry.azurecr.io/envoyproxy:1.0 .

2.  docker push yourregistry.azurecr.io/envoyproxy:1.0

### Create the secret for SSL certificates and deploy the deploy.yaml

0.  Create the secret containing the SSL Certificates for Envoy listener (*named envoycert.cer and envoycert.key*):

    ```kubectl create secret generic sslcert --from-file=cer=.\envoycert.cer --from-file=key=.\envoycert.key```

1.  Modify the deploy.yaml in order to:

    a.  Point to the proper registry
    b.  Have an Internal Load Balancer (*for testing purposes I commented the annotation to make the LB internal and not public*)

2.  Deploy the yaml with a  ```kubectl apply -f deploy.yaml```

3.  Retrieve the IP for the Load Balancer and set the DNS A records for keyvault.envoy.mydomain.com and storage.envoy.mydomain.com to point to that IP

4.  You are now ready to test the proxy with the sample C# console applications in the other folder of this repo