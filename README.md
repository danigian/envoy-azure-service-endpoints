# Use Envoy to reach Azure Service Endpoints from on-prem network (via VPN)

Virtual Network service endpoints allow you to secure some critical Azure services to only specific virtual networks. Though, there is a limitation: endpoints cannot be used for traffic from your premises to Azure services. Right now, if you want to allow traffic from on-premises, you must also allow public (typically, NAT) IP addresses from your on-premises or ExpressRoute.

In this repo you will find out how to allow your communications to securely go through your VPN by using Envoy as a proxy.

## Repository structure

There are two folders in this repository:

0. **envoy/** contains the proxy configuration, the Dockerfile to build the proper docker image and a Kubernetes YAML to deploy it to your cluster
1. **samples/** contains two sample console applications (for accessing KeyVault and Storage Accounts) 