# Check-it - Wartung

## Allgemeines

_Check-it_ besteht aus zwei Komponenten: `checkit_backend` und `checkit_frontend`, welche jeweils das Front- und Backend bereitstellen.
`checkit_backend` basiert auf .NET 6.0 mit ASP.NET Core. `checkit_frontend` auf Clientseitiger HTML-JS-Basis.

## Voraussetzungen

### Client

* Aktueller Webbrowser
* Netzwerkzugriff auf die zwei Ports (std. 80 u. 5000) des Servers

### Server

#### Image

* Virtualisierungslösung, die VHDX unterstützt
* Min. 2GB Arbeitsspeicher für die Virtuelle Maschine
* Freigabe der benutzten Ports für Clients 

#### Manuelle Installation

* Freigabe der benutzten Ports für Clients
* System mit Linux, macOS oder Windows 10+
* Software:
	* Webserver für statische Dateien
	* .NET 6.0 Runtime
	* ASP.NET Core Runtime
	
## Installation

### Image

Bereitgestellt wird ein VM-Image mit Ubuntu Server 22.04 LTS. Darin laufen als Docker-Services das Backend und Frontend (mittels Apache).
Hostname ist `checkit`.
Das Programm ist sofort einsatzbereit, sobald die Maschine hochgefahren ist.

#### Zugangsdaten

Auf dem Image ist SSH aktiviert. Der Standardbenutzer ist `checkit` mit Passwort `checkit`.

#### Wichtige Pfade

* `/home/checkit/checkit/`
	* `checkit.db` - SQLite-Datenbank mit den gespeicherten Daten von Check-it (wird bei ).
	* `docker-compose.yaml` - Docker-Compose-File, hier können Ports und DB-Pfade des konfiguriert werden.

#### Ports

| Service          | Port |
|------------------|------|
| checkit_frontend | 80   |
| checkit_backend  | 5000 |
| ssh              |      |

Sowohl der Port für `checkit_frontend` und `checkit_backend` muss von Clients aus erreichbar sein.

### Manuelle Installation

#### checkit_fronend

Ein eingerichteter Webserver wird vorausgesetzt
1. Bereitgestelltes Archiv entpacken
2. Dateien in ein Verzeichnis verschieben, das der Webserver serviert
3. In der Datei `backend_config.json` die Werte zur Backendkommunikation setzen.

#### checkit_backend

1. Bereitgestelltes Archiv entpacken.
2. Die Umgebungsvariable `ASPNETCORE_URLS` auf eine Listen-Adresse in der Form von `<protocol>://<address>:<port>` setzen. Z.B. `http://0.0.0.0:5000`.
3. awl_raumreservierung ausführen.

## Update

### checkit_backend

Updates werden als Archiv bereitgestellt.
1. Archiv entpacken.
2. install.sh ausführen.
3. Den Container aktualisieren und den Dienst neustarten (in `/home/checkit/checkit` den Befehl `docker-compose down && docker-compose up -d` ausführen).

## Konfiguration

### Image

Die Konfiguration des Images findet über die bereitgestelle Docker-Compose-Datei `docker-compose.yaml` (auf YAML-Basis) statt. [https://docs.docker.com/compose/](Weitere Informationen).
Die Datei ist für checkit_frontend und checkit_backend in zwei Sektionen geteilt. Konfigurierbare Werte sind mit Kommentaren erläutert.
Alle Änderungen werden in `/home/checkit/checkit/docker-compose.yaml` getätigt und nach dem Speichern mit `docker-compose up -d` (im selben Verzeichnis wie die Datei) übernommen.

#### checkit_frontend

##### Port

Unter `services` - `checkit_frontend` - `ports` die erste Zahl auf den Wunschport ändern. Z.B. `1234:8080` stellt das Frontend auf Port 1234 bereit.

#### checkit_backend

##### Port

Unter `services` - `checkit_backend` - `ports` die erste Zahl auf den Wunschport ändern. Z.B. `1234:5000` stellt den Dienst auf Port 1234 bereit.

##### Datenbankpfad

Unter `services` - `checkit_backend` - `volumes` den Pfad vor `:/data/checkit.db` auf den Wunschpfad ändern. Z.B. `~/ordner/checkit.db:/data/checkit.db` lässt den Dienst die Datenbank unter `~/ordner/checkit.db`speichern.

##### Backendverbindung

Unter `services` - `checkit_backend` - `environment` die Werte von `CHECKIT_BACKEND_PORT`, `CHECKIT_BACKEND_PROTOCOL` und `CHECKIT_BACKEND_ADDRESS` ändern. Dieser Port myuIst das Backend z.B. über http://192.168.1.20:5000 erreichbar, muss die Konfiguration folgendermaßen aussehen:
```yaml
- CHECKIT\_BACKEND\_PROTOCOL="http"  
- CHECKIT\_BACKEND\_PORT="5000"
- CHECKIT\_BACKEND\_ADDRESS="192.168.1.20" 
```