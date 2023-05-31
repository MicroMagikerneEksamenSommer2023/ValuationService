# API-dokumentation for Valuation Service

Nedenstående dokument beskriver de tilgængelige endpoints og deres funktionalitet i Valuation Service.

## Endpoints

### POST /valuationservice/v1/requestvaluation

**Beskrivelse:** Opretter en anmodning om vurdering af en katalogvare.

**Handling:** Denne endpoint opretter en anmodning om vurdering af en katalog item og gemmer den i databasen.

**Parametre:**

- `data` (type: JSON): Dataobjektet, der indeholder information om katalog item, der skal vurderes.
- `images` (type: Form File): En liste af billeder tilknyttet katalog item.

**Svar:**

- 200 OK: Hvis katalog item oprettelse lykkedes. Returnerer en bekræftelsesmeddelelse og status for oprettelsen.
- 500 Internal Server Error: Hvis der opstår en uventet fejl under oprettelsen.

### GET /valuationservice/v1/getall

**Beskrivelse:** Henter alle katalog item-anmodninger.

**Handling:** Denne endpoint henter alle eksisterende katalog item-anmodninger fra databasen.

**Svar:**

- 200 OK: Returnerer en liste af katalog item-anmodninger som JSON.
- 404 Not Found: Hvis der ikke findes nogen katalog item-anmodninger.
- 500 Internal Server Error: Hvis der opstår en uventet fejl under hentningen.

### GET /valuationservice/v1/getbyemail/{email}

**Beskrivelse:** Henter katalog items-anmodninger baseret på e-mail.

**Parametre:**

- `email` (type: string): E-mailadressen for brugeren.

**Handling:** Denne endpoint henter alle katalog items-anmodninger, der er tilknyttet den angivne e-mailadresse.

**Svar:**

- 200 OK: Returnerer en liste af katalog items-anmodninger som JSON.
- 404 Not Found: Hvis der ikke findes nogen katalog items-anmodninger for den angivne e-mailadresse.
- 500 Internal Server Error: Hvis der opstår en uventet fejl under hentningen.

### GET /valuationservice/v1/getpending

**Beskrivelse:** Henter alle ventende katalog items-anmodninger.

**Handling:** Denne endpoint henter alle ventende katalog items-anmodninger fra databasen.

**Svar:**

- 200 OK: Returnerer en liste af ventende katalog items-anmodninger som JSON.
- 404 Not Found: Hvis der ikke findes nogen ventende katalog items-anmodninger.
- 500 Internal Server Error: Hvis der opstår en uventet fejl under hentningen.

### PUT /valuationservice/v1/valuate

**Beskrivelse:** Udfører en vurdering af et katalog item.

**Handling:** Denne endpoint udfører en vurdering af et katalog item baseret på de angivne data.

**Parametre:**

- `data` (type: JSON): Dataobjektet, der indeholder information om vurderingen af katalog itemet.

**Svar:**

- 200 OK: Hvis vurderingen lykkedes. Returnerer vurderingsresultatet som JSON.
- 404 Not Found: Hvis katalog item ikke findes.
- 500 Internal Server Error: Hvis der opstår en uventet fejl under vurderingen.

### DELETE /valuationservice/v1/delete/{id}

**Beskrivelse:** Sletter et katalog item-anmodning baseret på ID.

**Parametre:**

- `id` (type: string): Den unikke ID for katalog item-anmodningen.

**Handling:** Denne endpoint sletter den katalog item-anmodning, der svarer til det angivne ID.

**Svar:**

- 200 OK: Hvis sletningen lykkedes. Returnerer en bekræftelsesmeddelelse.
- 404 Not Found: Hvis katalog item-anmodningen ikke findes.
- 500 Internal Server Error: Hvis der opstår en uventet fejl under sletningen.

## Modeller

### ValuationRequest

**Beskrivelse:** Representerer en katalog item-anmodning.

**Attributter:**

- `id` (type: string): Den unikke ID for katalog item-anmodningen.
- `name` (type: string): Navnet på katalog item.
- `description` (type: string): Beskrivelse af katalog item.
- `email` (type: string): E-mailadressen for brugeren, der anmoder om vurdering.
- `status` (type: string): Status for katalog item-anmodningen (f.eks. "pending", "evaluated").
- `valuation` (type: decimal): Vurderingsværdien for katalog item.

### ValuationResult

**Beskrivelse:** Representerer resultatet af en vurdering af en katalog item.

**Attributter:**

- `id` (type: string): Den unikke ID for katalog item.
- `name` (type: string): Navnet på katalog item.
- `description` (type: string): Beskrivelse af katalog item.
- `valuation` (type: decimal): Vurderingsværdien for katalog item.

## Fejlhåndtering

Valuation Service kan returnere følgende fejlmeddelelser:

- 400 Bad Request: Hvis der opstår en fejl med anmodningen eller de angivne data.
- 404 Not Found: Hvis en ressource ikke findes.
- 500 Internal Server Error: Hvis der opstår en uventet serverfejl.

# Valuation Service Models

Nedenstående dokument beskriver de tilhørende klasser og deres funktionalitet i ValuationService.

## Indhold

- [ItemsNotFoundException](#itemsnotfoundexception)
- [ImageResponse](#imageresponse)
- [JsonModelBinder](#jsonmodelbinder)
- [Valuation](#valuation)
- [ValuationDB](#valuationdb)
- [ValuationData](#valuationdata)

## CustomException -ItemsNotFoundException

En specialiseret undtagelsestype, der bruges til at angive en fejl, når der ikke findes elementer.

### Constructer 

- `ItemsNotFoundException()` - Initialiserer en ny instans af `ItemsNotFoundException`-klassen uden en besked.
- `ItemsNotFoundException(string message)` - Initialiserer en ny instans af `ItemsNotFoundException`-klassen med den angivne besked.
- `ItemsNotFoundException(string message, Exception inner)` - Initialiserer en ny instans af `ItemsNotFoundException`-klassen med den angivne besked og en henvisning til den indre undtagelse, der er årsagen til denne undtagelse.

## ImageResponse

En model, der repræsenterer svaret på en billedanmodning. Den indeholder en liste af byte-arrays, der repræsenterer filbyte-data, samt yderligere data af typen `Valuation`.

### Egenskaber

- `FileBytes` - En liste af byte-arrays, der repræsenterer filbyte-data.
- `AdditionalData` - Yderligere data af typen `Valuation`.

### Constructer

- `ImageResponse(List<byte[]> img, Valuation data)` - Initialiserer en ny instans af `ImageResponse`-klassen med den angivne liste af byte-arrays og `Valuation`-data.

## JsonModelBinder

En modelbinder, der konverterer JSON-data til et objekt ved hjælp af NewtonSoft.Json-biblioteket.

### Metoder

- `BindModelAsync(ModelBindingContext bindingContext)` - Binder JSON-data til et objekt ved at deserialisere den indkommende JSON-streng og opdatere modelbindingens resultat.

## Valuation

En model, der repræsenterer en vurdering (valuation). Den indeholder informationer som titel, beskrivelse, kundens e-mail, vurderingspris, vurderingsårsag og status.

### Egenskaber

- `Id` - Vurderingens ID.
- `Title` - Titlen på vurderingen.
- `Description` - Beskrivelsen af vurderingen.
- `CustomerEmail` - E-mailadressen til kunden.
- `ValuationPrice` - Vurderingsprisen.
- `ValuationReason` - Årsagen til vurderingen.
- `Status` - Status for vurderingen.

### Constructer

- `Valuation(string title, string description, string customerEmail)` - Initialiserer en ny instans af `Valuation`-klassen med den angivne titel, beskrivelse og kundens e-mail. ValuationPrice, ValuationReason og Status sættes til standardværdier.
- `Valuation(string id, string title, string description, string customerEmail, double valuationPrice, string valuationReason, string status)` - Initialiserer en ny instans af `Valuation`-klassen med den angivne ID, titel, beskrivelse, kundens e-mail, vurderingspris, vurderingsårsag og status.
- `Convert(List<string> paths)` - Konverterer `Valuation`-objektet til et `ValuationDB`-objekt ved at inkludere billedstierne.

## ValuationDB

En model, der repræsenterer en vurdering (valuation) i databasen. Den indeholder informationer som titel, beskrivelse, kundens e-mail, vurderingspris, vurderingsårsag, status og en liste over billedstier.

### Egenskaber

- `Id` - Vurderingens ID.
- `Title` - Titlen på vurderingen.
- `Description` - Beskrivelsen af vurderingen.
- `CustomerEmail` - E-mailadressen til kunden.
- `ValuationPrice` - Vurderingsprisen.
- `ValuationReason` - Årsagen til vurderingen.
- `Status` - Status for vurderingen.
- `ImagePaths` - En liste over billedstier.

### Constructor

- `ValuationDB(string title, string description, string customerEmail, double valuationPrice, string valuationReason, string status, List<string> imagePaths)` - Initialiserer en ny instans af `ValuationDB`-klassen med den angivne titel, beskrivelse, kundens e-mail, vurderingspris, vurderingsårsag, status og en liste over billedstier.
- `ValuationDB(string title, string description, string customerEmail, double valuationPrice, string valuationReason, string status)` - Initialiserer en ny instans af `ValuationDB`-klassen med den angivne titel, beskrivelse, kundens e-mail, vurderingspris, vurderingsårsag og status.
- `Convert()` - Konverterer `ValuationDB`-objektet til et `Valuation`-objekt.

## ValuationData

En model, der repræsenterer data for en vurdering (valuation). Den indeholder informationer som vurderingspris, vurderingsårsag og status.

### Egenskaber

- `Id` - Vurderingens ID.
- `ValuationPrice` - Vurderingsprisen.
- `ValuationReason` - Årsagen til vurderingen.
- `Status` - Status for vurderingen.

### Constructor

- `ValuationData(string id, double valuationPrice, string valuationReason, string status)` - Initialiserer en ny instans af `ValuationData`-klassen med den angivne ID, vurderingspris, vurderingsårsag og status.
