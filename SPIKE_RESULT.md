# Objetivo

Validar el entorno de desarrollo mediante una aplicación WinUI 3 mínima, empaquetada y ejecutable en Debug. El spike no incorpora MVVM, DI, logging, configuración ni funcionalidades de Oficialía.

# Entorno

- Visual Studio Community 2026, versión 18.8.2 (`18.8.12023.21`).
- .NET SDK `10.0.302`; MSBuild `18.6.11`.
- Sistema operativo real confirmado mediante `systeminfo`:
  - Microsoft Windows 11 Pro
  - build `26200`
  - x64
- Algunas API o herramientas pueden devolver identificadores heredados de Windows 10 por compatibilidad; esos identificadores no deben utilizarse como nombre comercial del sistema.
- Windows App SDK solicitado por la plantilla: `2.0.260206002-stable`.
- Windows App SDK efectivo restaurado: `2.1.3`.
- Plantilla: `Microsoft.WinUI.Desktop.Cs.SingleProjectPackagedApp`, C#, WinUI 3 Packaged, proyecto único.

# Proyecto generado

Solución y proyecto:

```text
Spike001.WinUI.sln
Spike001.WinUI/
├── Spike001.WinUI.csproj
├── App.xaml
├── App.xaml.cs
├── MainWindow.xaml
├── MainWindow.xaml.cs
├── Package.appxmanifest
├── app.manifest
├── Assets/
└── Properties/
    ├── launchSettings.json
    └── PublishProfiles/
        ├── win-arm64.pubxml
        ├── win-x64.pubxml
        └── win-x86.pubxml
```

No se añadieron componentes de aplicación fuera de la plantilla.

# Resultado de compilación

- Configuración: `Debug`, plataforma `x64`.
- Comando: `dotnet build Spike001.WinUI.sln --configuration Debug --property:Platform=x64`.
- Resultado: correcto, código `0`, `0` errores.
- Tiempo aproximado: `38.33 s`.
- Salida real: `C:\Desarrollo\oficialia-techlab\Spike001.WinUI\bin\x64\Debug\net10.0-windows10.0.19041.0\win-x64`.
- Tamaño de `C:\Desarrollo\oficialia-techlab\Spike001.WinUI\bin\x64\Debug`: `119,153,455 bytes` (`113.63 MiB`, 235 archivos).

El build emitió una advertencia NU1603 porque la versión de Windows App SDK declarada por la plantilla no estaba disponible en el feed; NuGet resolvió `2.1.3`. El empaquetado también indicó que `mspdbcmf.exe` no estaba disponible, por lo que no se generó un paquete de símbolos.

# Resultado de ejecución

- Se generó el paquete MSIX mediante `dotnet publish` con `GenerateAppxPackageOnBuild=true`.
- Paquete: `C:\Desarrollo\oficialia-techlab\Spike001.WinUI\AppPackages\Spike001.WinUI_1.0.0.0_x64_Debug_Test\Spike001.WinUI_1.0.0.0_x64_Debug.msix`.
- Instalación: `Install.ps1` generado por el empaquetado oficial; resultado confirmado como aplicación instalada correctamente.
- AppID utilizado: `8f7b0b7f-8f77-4f09-a1a9-001c47e3c9b1_z6r1p1ws80brm!App`.
- Primer arranque aproximado: `5.22 s`.
- Ventana observada: título `Spike001.WinUI`, handle válido y proceso respondiente.
- Cierre: `CloseMainWindow=True`; el proceso terminó después de la solicitud de cierre normal.
- No se observaron excepciones durante el arranque Packaged.

# Dependencias iniciales

Únicamente las dependencias incluidas por la plantilla:

- `Microsoft.WindowsAppSDK`: referencia declarada `2.0.260206002-stable`; versión efectiva `2.1.3`.
- `Microsoft.Windows.SDK.BuildTools`: `10.0.26100.4654`.

No se agregaron CommunityToolkit, MVVM, DI, logging, configuración, SQLite, Python ni otros paquetes.

# Observaciones

- La activación Packaged requirió el certificado temporal de desarrollo habitual para MSIX; se retiró al terminar la validación.
- La ejecución directa del `.exe` fuera del contexto de identidad Packaged no es una validación equivalente y produjo una excepción .NET en esta sesión; la activación oficial por AppID sí inició y cerró correctamente.
- El repositorio quedó sin commit ni push, en la rama `spike/001-winui`.

# Riesgos detectados

- La versión de Windows App SDK usada efectivamente (`2.1.3`) no coincide con la versión exacta declarada por la plantilla (`2.0.260206002-stable`) debido a disponibilidad del feed; debe fijarse/confirmarse en el siguiente bloque si se requiere reproducibilidad exacta.
- Falta `mspdbcmf.exe` en el entorno, lo que afecta únicamente la generación de símbolos del paquete.

# Conclusión

La plataforma base WinUI 3 Packaged en C#/.NET 10 compila en Debug, se despliega con el mecanismo oficial MSIX, abre una ventana funcional y se cierra normalmente. El alcance del spike queda validado; las advertencias de versión y símbolos quedan registradas como riesgos para bloques posteriores.

WinUI 3 no se presenta todavía como arquitectura definitivamente ratificada; la validación queda limitada al alcance de este spike y requiere continuar con la siguiente fase.

El riesgo residual es Medio por:
- NU1603 y resolución Windows App SDK 2.1.3;
- primer arranque ~5.22 s;
- modelo Packaged dependiente de identidad;
- estrategia de distribución todavía no validada.

Hipótesis validada: Sí
Evidencia suficiente: Sí
Riesgo residual: Medio
Recomendación: Continuar con SPIKE-001B

# SPIKE-001B — Arquitectura Base

## Objetivo

Validar una capa de presentación WinUI 3 pequeña basada en MVVM, CommunityToolkit.Mvvm, constructor injection, Microsoft.Extensions.DependencyInjection y navegación básica desacoplada, sin implementar funcionalidades de Oficialía.

## Hipótesis

WinUI 3 permite mantener ViewModels sin dependencia de Microsoft.UI.Xaml, un composition root explícito, navegación básica mediante contrato y code-behind limitado a responsabilidades visuales, sin introducir un framework arquitectónico pesado.

## Diseño probado

- App.xaml.cs construye el contenedor y crea MainWindow mediante constructor injection.
- MainViewModel expone tres comandos de navegación.
- INavigationService recibe únicamente NavigationDestination.
- NavigationService concreta la asociación con Frame y crea Pages mediante fábricas explícitas.
- HomePage, PlatformPage y TestsPage muestran únicamente su nombre y propósito experimental.
- No se implementaron dominio, persistencia, logging, archivos, reportes ni funcionalidades de Oficialía.

## Dependencias añadidas

- CommunityToolkit.Mvvm: solicitada 8.4.2, resuelta 8.4.2; se usa para ObservableObject, [ObservableProperty] y [RelayCommand].
- Microsoft.Extensions.DependencyInjection: solicitada 10.0.10, resuelta 10.0.10; se usa para ServiceCollection, registros explícitos y constructor injection.
- No se agregó Microsoft.Extensions.Hosting, Prism, Autofac, DryIoc ni otro framework equivalente.
- La dependencia existente Microsoft.WindowsAppSDK solicitada 2.0.260206002-stable continúa resolviendo 2.1.3 y conserva NU1603.

## Composition Root

App.xaml.cs actúa como composition root. Registra explícitamente los ViewModels singleton, las Pages transient, las fábricas de Pages, NavigationService, INavigationService, MainViewModel y MainWindow. El único acceso a IServiceProvider está en App.xaml.cs, durante la construcción y resolución del árbol de objetos.

## MVVM

HomeViewModel, PlatformViewModel y TestsViewModel heredan de ObservableObject y usan [ObservableProperty] para el texto experimental de propósito. MainViewModel usa [RelayCommand] para exponer los comandos del Shell. Ningún ViewModel crea Views, usa Frame, conoce MainWindow o accede al contenedor.

## Dependency Injection

La aplicación usa constructor injection en MainWindow, las tres Pages, MainViewModel y NavigationService. Los ViewModels se registran como singleton para evaluar preservación de instancia. Las Pages se registran como transient y se solicitan mediante fábricas explícitas, por lo que cada navegación crea una Page nueva sin recrear sus ViewModels singleton.

## Navegación

INavigationService expone únicamente Navigate(NavigationDestination), sin tipos WinUI. NavigationService es la implementación concreta de la capa de presentación: recibe fábricas de Pages, conserva el Frame asociado mediante Attach(Frame) y asigna Frame.Content. No utiliza reflection, rutas parametrizadas, deep links ni back stack avanzado.

## Code-behind residual

- App.xaml.cs: inicialización de XAML, configuración del contenedor, resolución y activación de MainWindow.
- MainWindow.xaml.cs: asignación del DataContext al Grid raíz, asociación del Frame con NavigationService y navegación inicial a Home.
- Cada Page code-behind: InitializeComponent y asignación del ViewModel recibido por constructor.

No quedan handlers de navegación en code-behind ni lógica funcional de Oficialía.

## Ciclo de vida de ViewModels

Los ViewModels se registran como singleton. Las Pages son transient y se crean a través de fábricas en cada navegación. No se agregó estado mutable de negocio ni un contador artificial; la prueba valida repetidamente que la navegación continúa funcionando con esta política de ciclo de vida. La preservación de instancia queda determinada por el registro singleton y la recreación de Page por el registro transient.

## Build

- Comando: dotnet build Spike001.WinUI.sln --configuration Debug --property:Platform=x64 --nologo.
- Código de salida: 0.
- Resultado: 0 errores y 2 advertencias, ambas emisiones de NU1603 durante restore/build.
- Tiempo aproximado del build final incremental: 1.96 s.
- Salida: Spike001.WinUI/bin/x64/Debug/net10.0-windows10.0.19041.0/win-x64.
- Tamaño actual: 119,547,028 bytes, 114.01 MiB, 241 archivos.
- SPIKE-001A: 119,153,455 bytes, 113.63 MiB, 235 archivos.
- Cambio aproximado: +393,573 bytes, +0.38 MiB, +6 archivos.

## Pruebas manuales

Se registró temporalmente el AppxManifest.xml de la salida x64 porque el MSIX publicado no tenía firma digital válida. La aplicación se activó con identidad Packaged y se comprobó mediante UI Automation:

1. Inició correctamente: Sí.
2. Home apareció: Sí.
3. Home → Platform: Sí.
4. Platform → Tests: Sí.
5. Tests → Home: Sí.
6. El ciclo completo se repitió cinco veces: Sí, 15 transiciones.
7. No se observaron excepciones durante la prueba: Sí.
8. No se crearon ventanas adicionales: Sí, un proceso y una ventana.
9. La UI continuó respondiendo: Sí en los cinco ciclos.
10. El servicio de navegación continuó operativo: Sí, todos los comandos funcionaron en cada ciclo.

La ejecución directa del .exe fuera de identidad Packaged volvió a fallar con la excepción conocida de SPIKE-001A. El MSIX publicó correctamente, pero Install.ps1 no pudo instalarlo por ausencia de firma digital válida.

## Fricciones encontradas

- Window no expone DataContext; el Shell usa el Grid raíz como DataContext.
- [ObservableProperty] sobre campos produjo MVVMTK0045 en WinUI; se cambió a propiedades parciales, desapareciendo esos warnings.
- La plantilla conserva NU1603 por resolución de Windows App SDK 2.1.3; no se corrigió ni parcheó.
- La validación Packaged depende de identidad y firma; el registro local fue utilizable, pero el MSIX no quedó instalable por falta de certificado.

## Riesgos

- NU1603 y reproducibilidad de Windows App SDK.
- Firma y distribución del MSIX todavía no validadas.
- El ciclo de vida singleton/transient queda probado en código y navegación, pero no contiene todavía estado de negocio real.
- La navegación intencionalmente no incluye back stack, deep links ni parámetros.

## Patrones candidatos

- PAT-001 — Constructor Injection: candidato para dependencias explícitas en composición y presentación.
- PAT-002 — CommunityToolkit.Mvvm: candidato para ObservableObject, ObservableProperty y RelayCommand.
- PAT-003 — Navigation Service desacoplado: candidato para separar destinos de la implementación WinUI.
- PAT-004 — App.xaml.cs como Composition Root: candidato para mantener el arranque explícito y pequeño.

Estos patrones son candidatos experimentales, no decisiones arquitectónicas definitivas.

Hipótesis validada: Sí
Evidencia suficiente: Sí
Riesgo residual: Medio
Recomendación: Continuar

# SPIKE-001C — Configuration Framework

## Objetivo

Validar una arquitectura de configuración fuertemente tipada, desacoplada y preparada para evolución, con persistencia local en `settings.json`, sin SQLite ni hosting.

## Diseño

La configuración se modela como `AppSettings` inmutable mediante records y propiedades `init`, con `ConfigurationVersion` y los submodelos de aplicación, apariencia, institución, correspondencia, numeración, OCR y diagnóstico. Los consumidores reciben un snapshot mediante `IConfigurationService.Current` y proponen cambios mediante un nuevo `AppSettings`; no pueden modificar libremente la instancia actual en sitio.

`SettingsSerializer` encapsula `System.Text.Json`. `SettingsValidator` valida valores y versión sin lanzar excepciones por errores esperados. `ConfigurationResult` separa `Errors`, `Warnings` e `Information`, y cada mensaje identifica si proviene de validación, serialización, sistema de archivos o servicio.

## Servicios

- `IConfigurationService`: expone `Current`, `Load`, `Save`, `Export` e `Import`.
- `ConfigurationService`: coordina validación, serialización, lectura y escritura atómica.
- `SettingsSerializer`: único componente que conoce el formato JSON.
- `SettingsValidator`: valida la configuración tipada y `ConfigurationVersion = 1`.

## Modelo

`AppSettings` contiene `ApplicationSettings`, `AppearanceSettings`, `InstitutionSettings`, `CorrespondenceSettings`, `NumberingSettings`, `OcrSettings` y `DiagnosticSettings`. No se utilizan diccionarios ni claves de configuración libres.

## Validación

Se ejecutó un arnés temporal fuera del producto contra el código de `Core/Configuration`:

- crear configuración por defecto: correcto;
- guardar `settings.json`: correcto;
- cargar: correcto;
- modificar una copia tipada: correcto;
- guardar nuevamente y recargar: correcto;
- confirmar persistencia de institución y estado de primer arranque: correcto;
- exportar e importar: correcto;
- importar JSON inválido: devuelve `InvalidJson`, conserva `Current` y no realiza ninguna escritura durante el rechazo.

El arnés temporal y sus artefactos fueron retirados después de la prueba; no forman parte del árbol final.

## Correcciones SPIKE-001C-CLOSE

- `ConfigurationService.Save` ahora realiza la limpieza del archivo temporal como operación best-effort: cualquier excepción de `File.Delete` queda contenida y no sustituye el resultado principal de `Save`. `Current` continúa actualizándose únicamente después de una escritura y reemplazo exitosos.
- Se agregó `ConfigurationIssueCode.ValidationSucceeded` y `SettingsValidator` lo utiliza para informar `"La configuración pasó la validación tipada."`; `ConfigurationLoaded` queda reservado para cargas exitosas.

Los valores `Prefix = "OF"`, `DefaultResponseDays = 5`, `RequireRecipient = true` y los demás valores por defecto son exclusivamente valores sintéticos del TechLab para validar el framework. No representan decisiones funcionales para Oficialía de Partes.

La revalidación de SPIKE-001C-CLOSE cubrió defaults, `Save`, `Load`, modificación inmutable, guardado y recarga, `Export`, `Import` y JSON inválido. Todos los casos pasaron. El build Debug x64 finalizó con código `0`, `0` errores y `2` advertencias NU1603 en `4.89 s`.

## Persistencia

La ruta predeterminada es `%LocalAppData%\OficialiaTechLab\settings.json`; las pruebas usaron una ruta temporal explícita. `Save` valida y serializa antes de tocar el destino, escribe en un archivo temporal exclusivo con UTF-8 sin BOM y `WriteThrough`, fuerza el flush del stream y después reemplaza el destino existente o mueve el temporal al destino nuevo. El temporal se elimina en `finally` si permanece.

Los errores de JSON o validación se devuelven en `ConfigurationResult` y no modifican `_current` ni escriben. Los fallos de lectura y escritura se devuelven separadamente como `FileReadFailed` o `FileWriteFailed`. Un JSON inválido no se sobrescribe ni provoca pérdida del archivo original.

`Export` prepara una representación JSON en memoria e `Import` valida primero y sólo actualiza el snapshot en memoria; guardar es explícito.

## Riesgos

- La escritura atómica depende de que el sistema de archivos permita reemplazo/movimiento en la ruta configurada.
- No se implementaron migraciones; `ConfigurationVersion` sólo prepara el contrato futuro.
- La configuración aún no está integrada con ViewModels ni UI, conforme al alcance.
- NU1603 de Windows App SDK continúa presente y no se modificó.

## Build

- Comando: `dotnet build Spike001.WinUI.sln --configuration Debug --property:Platform=x64 --nologo`.
- Código de salida: `0`.
- Errores: `0`.
- Advertencias: `2` emisiones de NU1603; Microsoft.WindowsAppSDK `2.1.3` se resolvió frente a la versión solicitada por la plantilla.
- Tiempo aproximado: `5.26 s` medidos por el comando; MSBuild reportó `5.06 s`.
- Salida: `Spike001.WinUI/bin/x64/Debug/net10.0-windows10.0.19041.0/win-x64`.
- Tamaño de salida Debug: `119,572,834 bytes` (`114.03 MiB`), `238` archivos.

## Conclusión

La configuración tipada, la validación separada y la persistencia segura son viables sin introducir hosting, SQLite ni frameworks adicionales. El resultado es un candidato para integración posterior; todavía no constituye una decisión definitiva sobre la arquitectura completa del producto.

Hipótesis validada: Sí
Evidencia suficiente: Sí
Riesgo residual: Medio
Recomendación: Continuar
