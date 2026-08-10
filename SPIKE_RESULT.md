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
