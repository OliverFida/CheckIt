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

#### Zugangsdaten

Auf dem Image ist SSH aktiviert. Der Standardbenutzer ist `checkit` mit Passwort `checkit`.

#### Wichtige Pfade

* `/home/checkit/checkit/`
	* `backend/`
		* `checkit.db` - Gespeicherte Daten von Check-it.
		* `docker-compose.yaml` - Docker-Compose-File, hier können Port und DB-Pfade des Backends konfiguriert werden.
	* `frontend/` 
		* `conf/` - Konfigverzeichnis von Apache.
		* `docker-compose.yaml` - Docker-Compose-File, hier können Port und Pfade des Frontends konfiguriert werden.

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

#### checkit_backend

1. Bereitgestelltes Archiv entpacken


## Update

### checkit_frontend

Updates werden als Archiv bereitgestellt.
1. Archiv in `/home/checkit/checkit/frontend/website` entpacken.

### checkit_backend

Updates werden als Archiv bereitgestellt.
1. Archiv entpacken.
2. install.sh ausführen.
3. Den Dienst neustarten (in `/home/checkit/checkit/backend/` den Befehl `docker-compose down && docker-compose up -d` ausführen).

## Konfiguration

### Image

Die Konfiguration des Images findet über die bereitgestellen Docker-Compose-Dateien statt. [https://docs.docker.com/compose/](Weitere Informationen).

#### checkit_frontend

##### Port

1. Datei `/home/checkit/checkit/frontend/docker-compose.yaml` öffnen
2. Unter `services` - `checkit_backend` - `ports` die erste Zahl auf den Wunschport ändern. Z.B. `1234:8080` stellt den Dienst auf Port 1234 bereit.
3. Mittels `docker-compose down && docker-compose up -d` den Dienst neustarten.
#### checkit_backend

##### Ports

1. Datei `/home/checkit/checkit/backend/docker-compose.yaml` öffnen.
2. Unter `services` - `checkit_backend` - `ports` die erste Zahl auf den Wunschport ändern. Z.B. `1234:5000` stellt den Dienst auf Port 1234 bereit.
3. Mittels `docker-compose down && docker-compose up -d` den Dienst neustarten.

##### Datenbankpfad

1. Datei `/home/checkit/checkit/backend/docker-compose.yaml` öffnen
2. Unter `services` - `checkit_backend` - `volumes` den Pfad vor `:/data/checkit.db` auf den Wunschpfad ändern. Z.B. `~/ordner/checkit.db:/data` lässt den Dienst die Datenbank unter `~/ordner/checkit.db`speichern.
3. Mittels `docker-compose down && docker-compose up -d` den Dienst neustarten.
