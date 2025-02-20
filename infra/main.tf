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


resource "azurerm_resource_group" "rg" {
  name     = "educationfcf_group"
  location = "Canada Central"
}

resource "azurerm_service_plan" "appserviceplan" {
  name                = "ASP-educationfcfgroup-9074"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  os_type             = "Linux"
  sku_name            = "F1"
}

resource "azurerm_windows_web_app" "webapp" {
  name                = "educationfcf"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  service_plan_id     = azurerm_service_plan.appserviceplan.id
  depends_on          = [azurerm_service_plan.appserviceplan]
  site_config {
    always_on               = true
    managed_pipeline_mode   = "Integrated"  
  }
}


resource "azurerm_mysql_flexible_server" "mysqlserver" {
  name                = "conchita"
  resource_group_name = "educationfcf_group"
  location            = azurerm_resource_group.rg.location
  administrator_login = var.sqladmin_username
  administrator_password = var.sqladmin_password
  sku_name            = "GP_Standard_D2ds_v4"
  storage {
    size_gb = 20  
  }
  version             = "8.0.21"
}

resource "azurerm_mysql_flexible_database" "db" {
  name                = "conchita"
  resource_group_name = azurerm_mysql_flexible_server.mysqlserver.resource_group_name
  server_name         = azurerm_mysql_flexible_server.mysqlserver.name
  charset            = "utf8mb4"
  collation          = "utf8mb4_general_ci"
}


resource "azurerm_dns_zone" "dns" {
  name                = "dns-proyecto-valverde-lizarraga.com"
  resource_group_name = azurerm_resource_group.rg.name
}

resource "azurerm_dns_cname_record" "cname" {
  name                = "www"
  zone_name           = azurerm_dns_zone.dns.name
  resource_group_name = azurerm_resource_group.rg.name
  ttl                 = 300
  record              = azurerm_windows_web_app.webapp.default_hostname
}
