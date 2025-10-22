1. FrameWork
Ein Framework ist ein vorgefertigtes Gerüst aus Code, Regeln und Werkzeugen, das dir hilft, Software schneller und strukturierter zu entwickeln. Es bietet dir eine feste Architektur, in die du deine eigene Logik einfügst – wie ein Baukasten mit klaren Steckplätzen.


2. Onion Architektur
Die Onion-Architektur besteht typischerweise aus mehreren konzentrischen Schichten:
	1. Core → Enthält abstrakte Klassen, Entitäten, Interfaces
	2. Repository/Persistence Layer --> Enthält die CRUD Methoden (DB-Zugriffe)
	3. Application Layer → Business-Logik (Validierung, Filter, DTO)
	4. Presentation Layer → UI, Web-API, MVC, MVVM Controller 
	
	
3. DBContext
Der DbContext ist das Herzstück von Entity Framework (EF) – er ist die zentrale Klasse, über die du mit der Datenbank arbeitest. Man kann ihn sich vorstellen wie eine Brücke zwischen deiner Anwendung und der Datenbank.

Hier sind seine Hauptaufgaben:
	• Verbindet zur Datenbank: Er weiß, wie er sich mit deiner Datenbank verbinden soll (über die Connection-String-Konfiguration).
	• Verwaltet Entitäten: Er hält alle geladenen Objekte im Speicher und verfolgt deren Zustand (neu, geändert, gelöscht, unverändert).
	• Führt Abfragen aus: Du kannst mit LINQ-Abfragen Daten aus der Datenbank lesen.
	• Speichert Änderungen: Mit SaveChanges() schreibt er alle Änderungen (Insert, Update, Delete) in die Datenbank.
	• Verwaltet Transaktionen: Du kannst mehrere Operationen bündeln und gemeinsam committen oder zurückrollen.


4. Dependency Injection
Dependency Injection (DI) in C# ist ein Entwurfsmuster, das dafür sorgt, dass Klassen ihre Abhängigkeiten nicht selbst erstellen, sondern von außen bereitgestellt bekommen. Das macht deinen Code flexibler, testbarer und besser wartbar.
Eine Abhängigkeit ist z. B. ein Objekt, das eine Klasse braucht, um zu funktionieren.

Vorteile von Dependency Injection
	• Lose Kopplung: Klassen sind nicht fest miteinander verbunden.
	• Einfaches Testen: Du kannst Abhängigkeiten durch Mocks ersetzen.
	• Wiederverwendbarkeit: Du kannst Komponenten austauschen, ohne andere zu ändern.
	• Zentrale Konfiguration: Du steuerst alles über den DI-Container.


5. Generische Methoden
Methoden, die mit Platzhaltern für Datentypen arbeiten – also nicht auf einen konkreten Typ festgelegt sind, sondern zur Laufzeit mit einem beliebigen Typ verwendet werden können. Das macht sie flexibel, wiederverwendbar und typsicher.

public T Echo<T>(T input)
{
    return input;
}
var result1 = Echo<int>(42);         // T = int
var result2 = Echo<string>("Hallo"); // T = string
	• T direkt nach public → Rückgabetyp der Methode.
	• <T> → Typparameterdefinition: Hier sagst du, dass T ein generischer Typ ist.
	• T input → Eingabeparameter vom Typ T

Vorteile:
	• Wiederverwendbarkeit: Du musst nicht für jeden Typ eine eigene Methode schreiben.
	• Typsicherheit: Der Compiler prüft zur Compile-Zeit, ob die Typen passen.


6. REST-API
Eine REST-API ist eine standardisierte Schnittstelle, über die Software-Systeme über das Internet miteinander kommunizieren.
Representational Application Programming Interface

	• Sie nutzt das HTTP-Protokoll (wie Webseiten), um Daten zu senden und zu empfangen.
	• Sie arbeitet mit Ressourcen (z. B. „Filme“, „Benutzer“, „Bestellungen“), die über URLs angesprochen werden.
	• Sie verwendet HTTP-Methoden:
		○ GET → Daten lesen
		○ POST → Neue Daten erstellen
		○ PUT / PATCH → Daten aktualisieren
		○ DELETE → Daten löschen
		
Die Daten selbst werden in Formaten JSON (üblich, leichtgewichtig), XML (strukturiert, mehr Overhead) übertragen.

REST definiert wie man kommuniziert, JSON/XML definieren was man überträgt.

Rest-API definiert die Schnittstelle. Die Übertragung der Daten über die REST-API erfolgt in Formaten wie JSON (optimaler) oder XML (mehr Overhead)

POST /api/movies
Content-Type: application/json
{
  "title": "Inception",
  "year": 2010
}
	• Die REST-API sagt: „Du kannst einen Film unter /api/movies anlegen.“
	• Das JSON beschreibt: „Das ist der Film, den ich dir sende.“





