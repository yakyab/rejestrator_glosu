# Rejestrator Głosu

Aplikacja do nagrywania dźwięku, która umożliwia użytkownikowi zapisywanie i odtwarzanie nagrań głosowych.

## Funkcje

- Nagrywanie dźwięku w czasie rzeczywistym.
- Odtwarzanie zapisanych nagrań.
- Wizualizacja poziomu głośności w czasie rzeczywistym.
- Możliwość zapisywania nagrań do plików formatu `.wav`.
- Obsługa błędów.
- Interfejs użytkownika oparty na WPF, prezentujący fale dźwiękowe i kontrolki do nagrywania/odtwarzania.

## Stack Technologiczny

- **Język:** C#
- **Framework:** .NET Framework
- **Interfejs:** WPF (Windows Presentation Foundation)
- **Biblioteka audio:** NAudio
- **Architektura:** MVVM (Model-View-ViewModel)

## Instalacja

1. Sklonuj to repozytorium na swój lokalny komputer.
2. Otwórz projekt w Visual Studio.
3. Zbuduj projekt i uruchom go, korzystając z opcji "Uruchom" w Visual Studio.

## Użytkowanie

1. Po uruchomieniu aplikacji, zobaczysz interfejs z przyciskami do kontroli nagrywania i odtwarzania.
2. Kliknij przycisk "Record", aby rozpocząć nagrywanie.
3. Kliknij przycisk "Stop", aby zakończyć nagrywanie.
4. Aby odtworzyć nagranie, kliknij przycisk "Play".
5. Jeśli chcesz zapisać nagranie, kliknij przycisk "Save" i wybierz lokalizację zapisu.
6. Aby wyczyścić aktualne nagranie i zresetować interfejs, kliknij przycisk "Clear".
