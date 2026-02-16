# Samvad XR — Culturally Aware Language Immersion in VR

> Learn a language the way it's actually spoken — through culture, context, and conversation.

Samvad XR is a VR application that transforms language learning from rote memorization into an immersive, culturally grounded experience. Users step into real-world scenarios — starting with **negotiating with a street vendor** — and practice speaking in a target language while an AI-powered NPC responds with culturally appropriate dialogue, gestures, and emotions in real time.

---

## The Problem

Traditional language-learning tools teach vocabulary and grammar but ignore the **unspoken rules** of communication. A negotiation in Tokyo looks entirely different from one in Tamil Nadu or New York — the gestures, the polite indirectness, and the acceptable haggling strategies are deeply cultural nuances that flashcards cannot teach.

Learners end up knowing *what* to say, but not *how*, *when*, or *why* to say it.

## The Solution

<img width="1072" height="509" alt="image" src="https://github.com/user-attachments/assets/16f974f1-a766-4c1c-802e-b185e85565a3" />


Samvad XR bridges this gap by providing a safe, immersive VR environment where users **learn by doing**:

- **Context Over Content** — Instead of isolated words, users face real-world scenarios with a culturally aware AI vendor.
- **Cultural RAG Engine** — The backend uses Retrieval-Augmented Generation grounded in cultural etiquette data. The AI adapts its behavior based on the target culture.
- **State-Aware Interaction** — A dynamic negotiation state machine (`INITIAL → INQUIRY → HAGGLING → DEAL`) drives the vendor's behavior, with a real-time **Happiness Score** that reacts to your offers, tone, and cultural adherence.
- **Multilingual Support** — Speak in English, Hindi, Tamil, Kannada, or Punjabi and learn any of the supported target languages.

---

## Architecture

<img width="1622" height="810" alt="image" src="https://github.com/user-attachments/assets/0c888b43-cada-4da0-a361-32c47befaee8" />


---

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Engine | Unity 6 (6000.0.x) with URP |
| VR SDK | OpenXR 1.14 + XR Interaction Toolkit 3.0 |
| Input | Unity Input System (controller buttons for record / cancel) |
| Audio | 16 kHz WAV capture via `Microphone` API, WAV ↔ AudioClip utility |
| Networking | `UnityWebRequest` — JSON payload with base64-encoded audio |
| UI | TextMeshPro, Unity UI (dropdowns, sliders, canvases) |
| Backend | Python API (Whisper STT, RAG pipeline, TTS), exposed via ngrok |
| Target Platform | Meta Quest (Android / ARM64) |

---

## Project Structure

```
Assets/
├── Animations/          # NPC talking animation controller & clips
├── Models/              # NPC character model (FBX)
├── Prefabs/             # Grabbable produce items (Apple, Orange, Tomato, …)
│                        # XR Origin rig
├── Scenes/
│   └── demo_city_night  # Main street-vendor night-market scene
├── Scripts/
│   ├── SimpleBackendConnector.cs   # Core API client — sends audio, parses response
│   ├── ReplayAudio.cs              # Mic recording controller (B = toggle, A = cancel)
│   ├── UIManager.cs                # Language-selection dropdowns
│   ├── NegotiationStateManager.cs  # Displays current negotiation phase
│   ├── SliderUpdate.cs             # Binds happiness score to UI slider
│   ├── NPCTalkingAnimation.cs      # Drives talk animation from AudioSource
│   ├── FaceCameraYOnly.cs          # Billboard UI panels toward headset
│   └── SoundPlayer.cs              # Generic SFX trigger
└── Resources/           # Runtime-loaded assets
```

---

## API Contract

**Request** `POST /api/test`

```json
{
  "audioData":         "<base64 WAV>",
  "inputLanguage":     "en",
  "targetLanguage":    "hi",
  "object_grabbed":    "Tomato",
  "happiness_score":   50,
  "negotiation_state": "INITIAL"
}
```

**Response**

```json
{
  "reply":                            "Vanakkam sagodharaa! Vaanga vaanga...",
  "replyEnglish":                     "Hello brother! Come come...",
  "audioReply":                       "<base64 TTS WAV>",
  "negotiation_state":                "INQUIRY",
  "happiness_score":                  55,
  "suggested_user_response":          "Bhaiya, thoda aur kam karo na?",
  "suggested_user_response_english":  "Brother, reduce the price a bit more?"
}
```

---

## Getting Started

### Prerequisites

- **Unity 6** (6000.0.53f1 or later) with Android Build Support (ARM64)
- **Meta Quest** headset (or any OpenXR-compatible HMD)
- Backend server running and reachable (update the URL in `SimpleBackendConnector`)

### Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/<your-org>/samvad-xr.git
   ```
2. Open the project in Unity Hub.
3. Set the backend URL in the `SimpleBackendConnector` component on your scene's manager object.
4. Select language pair via the in-VR dropdown UI.
5. Build & Run to Quest via **File → Build Settings → Android**.

### VR Controls

| Button | Action |
|--------|--------|
| **B** (right controller) | Start / stop recording & send to backend |
| **A** (right controller) | Cancel current recording |
| **Grip** | Grab produce items to set negotiation context |

---

## Features

- **Real-time voice conversation** — speak naturally; Whisper transcribes, RAG responds, TTS speaks back.
- **Dynamic NPC emotions** — happiness slider and negotiation state update live based on your cultural performance.
- **Animated vendor NPC** — lip-sync-style talk animation driven by audio playback.
- **Suggested responses** — after 20 seconds of inactivity, the app surfaces a culturally appropriate hint to keep the conversation flowing.
- **Bilingual feedback** — AI replies and suggestions shown in both the target language and English.
- **Grabbable context objects** — pick up items in the scene to shift the topic of negotiation.

---



