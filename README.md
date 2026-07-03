# Sistem za upravljanje treningom i razvojem zaposlenih

Aplikacija predstavlja informacioni sistem namenjen upravljanju procesima obuke, razvoja i stručnog usavršavanja zaposlenih u organizaciji.

Sistem omogućava evidenciju zaposlenih, predavača, kategorija i razvojnih programa, prijavljivanje zaposlenih na programe, obradu prijava, praćenje rezultata i generisanje PDF izveštaja.

## Tehnologije

- ASP.NET Core Web API
- .NET 9
- Entity Framework Core
- SQL Server LocalDB
- ASP.NET Core Identity
- JWT autentifikacija i autorizacija
- MediatR
- CQRS obrazac
- FluentValidation
- Repository obrazac
- Unit of Work obrazac
- QuestPDF
- xUnit
- Moq

## Arhitektura rešenja

Backend deo rešenja podeljen je na četiri projekta:

- `TreningIRazvoj.API` – Web API, kontroleri, DTO klase, validacija, CQRS i servisi
- `TreningIRazvoj.Domen` – domenski modeli, enumeracije i interfejsi
- `TreningIRazvoj.Infrastruktura` – Entity Framework Core, repozitorijumi, Identity i pristup bazi
- `TreningIRazvoj.Testovi` – jedinični testovi

Pored backend projekata, solution sadrži i pomoćni projekat `TreningIRazvoj.Frontend`, koji služi za demonstraciju funkcionalnosti Web API-ja.

## Funkcionalnosti

### Zaposleni

- prikaz svih zaposlenih;
- prikaz zaposlenog po identifikatoru;
- kreiranje zaposlenog;
- izmena zaposlenog;
- brisanje zaposlenog;
- provera jedinstvenosti imejl adrese.

### Predavači

- prikaz predavača;
- kreiranje predavača;
- izmena predavača;
- brisanje predavača.

### Kategorije razvojnih programa

- prikaz kategorija;
- kreiranje kategorije;
- izmena kategorije;
- brisanje kategorije.

### Razvojni programi

- prikaz svih programa;
- prikaz programa po identifikatoru;
- kreiranje programa;
- izmena programa;
- brisanje programa;
- pretraga;
- filtriranje;
- sortiranje;
- straničenje rezultata.

Podržane vrste programa:

- obuka;
- OnBording;
- radionica;
- sertifikacija;
- mentorstvo.

### Prijave zaposlenih

Prijavama zaposlenih upravljaju korisnici sa ulogama `Administrator` i `HR`.

Podržane funkcionalnosti:

- prijavljivanje zaposlenog na razvojni program;
- pregled svih prijava;
- pregled pojedinačne prijave;
- odobravanje i odbijanje prijave;
- evidentiranje završetka programa;
- unos procenta prisustva;
- unos broja poena;
- unos konačne ocene završenog programa.

Podržani statusi prijave:

- NaCekanju;
- Odobrena;
- Odbijena;
- Zavrsena;
- Otkazana.

### Autentifikacija i autorizacija

Sistem koristi ASP.NET Core Identity i JWT tokene.

Podržane uloge:

- `Administrator`
- `HR`
- `Zaposleni`

Konačna podela ovlašćenja je sledeća:

- `Administrator` ima potpuni pristup sistemu, uključujući registraciju korisničkih naloga;
- `HR` upravlja zaposlenima, predavačima, kategorijama, razvojnim programima i prijavama zaposlenih;
- `Zaposleni` ima pristup pregledu razvojnih programa, kategorija programa i predavača, ali nema pristup administrativnom upravljanju prijavama.

Korisnički nalog sa ulogom `Zaposleni` i poslovni entitet `Zaposleni` predstavljaju odvojene delove sistema. U trenutnoj verziji sistema HR i Administrator vode administrativni proces prijavljivanja konkretnih zaposlenih na razvojne programe.

Autorizacija se sprovodi na serverskoj strani korišćenjem atributa `Authorize` i korisničkih uloga. Korisniku koji nema odgovarajuću ulogu API vraća status `403 Forbidden`.

### PDF izveštaj

QuestPDF se koristi za generisanje izveštaja o prijavama zaposlenih.

Izveštaj sadrži:

- zaposlenog;
- razvojni program;
- datum prijave;
- status;
- procenat prisustva;
- ocenu programa;
- datum generisanja;
- numeraciju stranica.

PDF izveštaj obuhvata sve evidentirane prijave i dostupan je korisnicima sa ulogama `Administrator` i `HR`.

## Validacija i obrada grešaka

Ulazni podaci proveravaju se pomoću biblioteke FluentValidation.

Implementiran je globalni middleware za obradu izuzetaka koji vraća odgovarajuće HTTP statuse:

- `400 Bad Request`
- `401 Unauthorized`
- `403 Forbidden`
- `404 Not Found`
- `500 Internal Server Error`

## Jedinični testovi

Za testiranje se koriste xUnit i Moq.

Implementirani su testovi:

- validatora za kreiranje zaposlenog;
- uspešnog kreiranja zaposlenog;
- sprečavanja kreiranja zaposlenog sa postojećom imejl adresom;
- provere imejl adrese nezavisno od velikih i malih slova.

Trenutni rezultat:

```text
10 Passed
0 Failed
```

## Pokretanje projekta

### 1. Kloniranje repozitorijuma

```bash
git clone https://github.com/stojilkovicca/sistem-za-upravljanje-treningom-i-razvojem-zaposlenih.git
```

### 2. Otvaranje rešenja

Otvoriti solution fajl:

```text
TreningIRazvoj.sln
```

### 3. Podešavanje JWT ključa

U terminalu otvorenom u folderu projekta:

```text
TreningIRazvoj.API
```

pokrenuti sledeće naredbe:

```bash
dotnet user-secrets init
dotnet user-secrets set "Jwt:Kljuc" "OVDE_UNETI_DUGACAK_TAJNI_KLJUC"
```

JWT ključ treba da bude dovoljno dug i složen.

### 4. Kreiranje baze podataka

U Visual Studio okruženju otvoriti:

```text
Tools → NuGet Package Manager → Package Manager Console
```

Zatim pokrenuti:

```powershell
Update-Database -Project TreningIRazvoj.Infrastruktura -StartupProject TreningIRazvoj.API
```

Ovom naredbom se primenjuju postojeće Entity Framework Core migracije i kreira lokalna baza podataka.

### 5. Pokretanje aplikacije

Kao startup projekat izabrati:

```text
TreningIRazvoj.API
```

Zatim aplikaciju pokrenuti preko HTTPS profila.

Podrazumevana lokalna HTTPS adresa je:

```text
https://localhost:7133
```

Port se može razlikovati u zavisnosti od lokalnog `launchSettings.json` fajla.

## Autor

**Aleksandar Stojilković**

Fakultet organizacionih nauka  
Informacioni sistemi i tehnologije