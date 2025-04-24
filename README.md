# ImeInterop

A lightweight Windows Forms library for capturing IME composition messages (e.g. Korean, Japanese, Chinese input) in real time.  
Supports multiple controls via centralized management.

---

## ✨ Features

- Hook `WM_IME_COMPOSITION` and related IME messages in `WinForms`
- Capture live composition text (`조합 중 문자열`)
- Handle IME start / end events
- Works with multiple controls via `ImeManager`
- No external dependencies

---

## 🔧 Usage Example

```csharp
var imeManager = new ImeManager();
imeManager.Attach(textBox1,
    onComposition: (s, e) => Console.WriteLine("Composing: " + e.CompositionText),
    onStart: (s, _) => Console.WriteLine("IME started"),
    onEnd: (s, _) => Console.WriteLine("IME ended"));
```

---

## 🧩 Components

| File | Purpose |
|------|---------|
| `ImeMessageInterceptor.cs` | Hooks IME messages via `NativeWindow` |
| `ImeManager.cs` | Manages multiple interceptors |
| `ImeCompositionEventArgs.cs` | Standardized event data |
| `ImeUtilities.cs` | Utility for language/encoding detection |

---

## 📜 License

MIT License. See [LICENSE](./LICENSE) for details.
