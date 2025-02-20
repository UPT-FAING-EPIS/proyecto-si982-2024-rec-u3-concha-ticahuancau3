terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 4.0.0"
    }
  }
  required_version = ">= 0.14.9"
}

variable "suscription_id" {
    type = string
    description = "a7e36467-e4ab-4a3c-8ac1-01b78c2f06d3"
}

variable "sqladmin_username" {
    type = string
    description = "conchita"
}

variable "sqladmin_password" {
    type = string
    description = "Hghjgyh3434@@"
}

provider "azurerm" {
  features {
    resource_group {
      prevent_deletion_if_contains_resources = false
    }
  }
  subscription_id = var.suscription_id
}


data "azurerm_resource_group" "rg" {
  name = "educationfcf_group"
}

data "azurerm_service_plan" "appserviceplan" {
  name                = "ASP-educationfcfgroup-9074"
  resource_group_name = data.azurerm_resource_group.rg.name
}

data "azurerm_windows_web_app" "webapp" {
  name                = "educationfcf"
  resource_group_name = data.azurerm_resource_group.rg.name
}

data "azurerm_mysql_flexible_server" "mysqlserver" {
  name                = "conchita"
  resource_group_name = data.azurerm_resource_group.rg.name
}

data "azurerm_mysql_flexible_database" "db" {
  name                = "conchita"
  resource_group_name = data.azurerm_mysql_flexible_server.mysqlserver.resource_group_name
  server_name         = data.azurerm_mysql_flexible_server.mysqlserver.name
}


resource "azurerm_dns_zone" "dns" {
  name                = "dns-educacionfcf.com"
  resource_group_name = "educationfcf_group"
}

resource "azurerm_dns_cname_record" "cname" {
  name                = "www"
  zone_name           = azurerm_dns_zone.dns.name
  resource_group_name = "educationfcf_group"
  ttl                 = 300
  record              = azurerm_windows_web_app.webapp.default_hostname
}

