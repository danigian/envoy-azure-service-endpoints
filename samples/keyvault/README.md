## KeyVault Set/Get secret through Envoy Proxy

This sample tries to Set and then Get the secret named "testSecret" (for which the value will be "secretValue")

### How to make it work

0.  The KeyVault needs to be locked down to the VNet in which your AKS cluster is deployed (info [here](https://docs.microsoft.com/en-us/azure/key-vault/key-vault-network-security))

1.  Create a Service Principal

    ```az ad sp create-for-rbac --skip-assignment -n "MyKVSP"```

    and take note of the output

2.  Assign a get/set/list secret access-policy to the newly created Key Vault

    ```az ad sp show --id http://MyKVSP --query objectId``` in order to retrieve the objectId for your Service Principal
    
    ```az keyvault set-policy -n <KEYVAULT_NAME> --object-id <SERVICE_PRINCIPAL_OBJECT_ID> --secret-permissions get set list```

3.  Set the following environment variables

    `proxyUrl`: the Envoy Proxy URL (i.e.: "https://keyvault.envoy.mydomain.com/")

    `akvClientId`: Service Principal Client ID (different than the Object ID)
    
    `akvClientSecret`: Service Principal Client Secret
    
    `akvTenantId`: The Tenant Id in which the Service Principal is located
    
    `akvSubscriptionId`: The Subscription ID in which the Key Vault is located

4.  Run!