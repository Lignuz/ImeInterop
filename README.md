# ImeInterop

A lightweight Windows Forms library for capturing IME composition messages (e.g. Korean, Japanese, Chinese input) in real time.  
Supports multiple controls via centralized management.

---

## ✨ Features

- Hook `WM_IME_COMPOSITION` and related IME messages in `WinForms`
- Capture live composition text (`조합 중 문자열`), including **empty string updates during deletion**
- Detect composition end via both `WM_IME_ENDCOMPOSITION` and when **composition string is cleared**
- Use Unicode-safe `ImmGetCompositionStringW` API for accurate multi-language IME support
- Handle IME start / end events reliably
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

## 🧠 IME Composition Flow

- `OnImeStartComposition`: Triggered when composition begins (WM_IME_STARTCOMPOSITION)
- `OnImeComposition`: Triggered on every composition update — **including empty string** (e.g. user pressed Backspace)
- `OnImeEndComposition`: Triggered either:
  - When WM_IME_ENDCOMPOSITION is received
  - Or when the composition string becomes empty

---

## 🧩 Components

| File | Purpose |
|------|---------|
| `ImeMessageInterceptor.cs` | Hooks IME messages and processes composition flow with Unicode support |
| `ImeManager.cs` | Manages multiple interceptors |
| `ImeCompositionEventArgs.cs` | Standardized event data |
| `ImeUtilities.cs` | Utility for IME context and language info (no longer used for encoding detection) |

---

## 📜 License

MIT License. See [LICENSE](./LICENSE) for details.
