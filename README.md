# RadioTests

## Wymagania
Przed uruchomieniem aplikacji upewnij się, że masz zainstalowane:
- [.NET 9](https://dotnet.microsoft.com/download/dotnet/9.0)
można sprawdzić status instalacji za pomocą:
    ```sh
    dotnet --info
    ```
## Instalacja i konfiguracja

1. **Sklonuj repozytorium:**
   ```sh
   git clone https://github.com/DZacheja/SimpleCRUD.git
   cd SimpleCRUD
   ```

2. **Przywróć zależności:**
   ```sh
   dotnet restore
   ```

## Uruchamianie aplikacji

### 1. Uruchomienie na lokalnym środowisku
```sh
dotnet run --project Radio.API
```

## Testowanie
Aby uruchomić testy jednostkowe, użyj:
```sh
dotnet test
```

## API Endpoints
Po uruchomieniu aplikacji API jest dostępne pod adresem:
```
https://localhost:5080/api/[url]
```
Jeśli używasz Swaggera, dokumentacja API będzie dostępna pod:
```
https://localhost:5080/swagger
```
## Zastosowane technologie
- Minimal API: Podejście w .NET do tworzenia prostych i lekkich API przy użyciu minimalnej ilości kodu. Jest to szczególnie przydatne dla małych aplikacji lub mikroserwisów, gdzie pełna struktura ASP.NET Core MVC może być zbyt rozbudowana.
- Swagger: Umożliwia automatyczne generowanie dokumentacji API i interaktywne testowanie endpointów.
- Entity Framework In-Memory Database: Prosta baza danych, której dane nie są przechowywane po zamknięciu aplikacji. W połączeniu z Seederem opartym o bibliotekę Bogus znacząco ułatwia testy podczas tworzenia aplikacji. Nie wymaga ona migracji baz danych więc można w szybki sposób zmieniać konfigurację bazy i sprawdzać czy wszystko działa poprawnie, bez konieczności przechowywania pierwszych, często zawierających błedy wersji bazy danych. Ponieważ korzysta z EF, późniejsza migracja tej bazy np do SQL Server wymaga jedynie podstawienia odpowiedniego connection string i przeprowadzeniu migracji.
- Clean Architecture: Wzorzec architektoniczny, który promuje separację warestw (separation of concerns) i niezależność od frameworków. Głównym celem jest stworzenie systemu, który jest łatwy do utrzymania, testowania i rozwijania. Struktura projektu jest podzielona na odpowiednie warstwy.
- Testy jednostkowe: Podstwowe testy jednostkowe do najbardziej rozbudowanego serwisu oraz repozytorium.
- Moq: Tworzenia obiektów zastępczych (mocków) w testach jednostkowych w .NET. Umożliwia symulowanie zachowania zależności, co pozwala na izolowanie testowanej logiki od zewnętrznych zależności, takich jak bazy danych, usługi sieciowe czy inne komponenty.
- Dependency Injection: Wstrzykiwanie zależności (Dependency Injection) jest używane do zarządzania zależnościami między komponentami aplikacji. DI ułatwia testowanie, ponieważ zależności mogą być łatwo mockowane. W projekcie zależności takie jak IRadioProgramRepository, IGenericService<ProgramDetails>, ILogger<RadioProgramService> są wstrzykiwane do serwisów, co pozwala na ich łatwe zastąpienie w testach.
- NLog: Popularna biblioteka do logowania w aplikacjach .NET. Umożliwia rejestrowanie zdarzeń, błędów i innych informacji diagnostycznych w różnych miejscach, takich jak pliki, bazy danych, konsola i wiele innych. NLog jest elastyczny i łatwy w konfiguracji, co czyni go jednym z najczęściej używanych narzędzi do logowania w ekosystemie .NET. Tutaj w celu prostoty został skonfigurowany na logowanie danych do konsoli (plik nlog.config).
## Możliwości rozbudowy
- Doanie autoryzacji i autentykacji w celu ograniczenia możliwośći subskrypcji na dane zadrzenie
- Dodanie wywoływania wielkowątkowego do Weebhook aby przyśpieszyć działanie aplikacji przy dużej ilości subskrypcji
- Dodanie większego pokrycia logami
- Dodanie konteneryzacji
- Rozbudowa Pipeline o Continous Delivery
---
Autor: **Damian Zacheja**
Cel: Rekrutacja

---
Po sprawdzeniu zadania proszę o kontakt w celu usunięcia repozytorium.
