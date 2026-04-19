# Future Plans

A prioritised backlog of features for future development of Timtek SAR, derived from the [[Competitive Comparison]] gap analysis. Features are scored on two axes and ranked by combined score:

- **Impact** — How much value does this deliver to users and the product's competitive position? (1 = marginal, 5 = transformative)
- **Ease** — How achievable is this relative to development effort, technical complexity, and external dependencies? (1 = major undertaking, 5 = quick win)

**Priority Score** = Impact + Ease (max 10). Higher is better. Ties are broken in favour of higher Impact.

## Priority 1 — Quick Wins with High Impact

### 1. Lost Pet Flyer Generation

| Impact | Ease | Score |
|:---:|:---:|:---:|
| 3 | 5 | **8** |

Generate a printable lost pet flyer (PDF) from case data — animal photo, description, last seen location (with W3W), case reference, and contact details. Keepers can download and print immediately upon case creation.

- **Justification:** Three competitors offer this (PawBoost, Pet FBI, Findpet). Extremely low effort — a PDF template populated from existing case data. High visibility for keepers who want to take immediate action in the physical world.
- **Approach:** Server-side PDF generation from case record. No new data capture required.

### 2. Social Media Integration — Facebook

| Impact | Ease | Score |
|:---:|:---:|:---:|
| 5 | 3 | **8** |

Automated posting of case alerts and status updates to configured Facebook pages or groups via the Facebook Graph API. Already identified in [[Functional Specification#8. Future Considerations]].

- **Justification:** This is the single biggest gap for public awareness. PawBoost's entire value proposition is built on Facebook reach (5.5M followers, 100M+ views/month). UK pet SAR groups already coordinate heavily through Facebook (e.g., Drone SAR For Lost Dogs UK). Automating what teams currently do manually would be a significant time saver.
- **Approach:** OAuth integration with Facebook Graph API. Configurable per organisation — link one or more Facebook pages/groups. Post on case creation, status change, and resolution. Include animal photo, description, location, and a link back to the public case status page.
- **Dependencies:** Facebook API approval, page/group admin permissions.

### 3. Progressive Web App (PWA)

| Impact | Ease | Score |
|:---:|:---:|:---:|
| 4 | 4 | **8** |

Add a service worker and web app manifest to enable installation as a PWA on mobile devices. This addresses two gaps simultaneously: native-app-like experience and basic offline capability.

- **Justification:** Four competitors have native apps. A PWA is a fraction of the effort — no app store submission, no separate codebase. Enables home screen installation, push notifications via the browser, and offline caching of recently viewed case data and maps.
- **Approach:** Service worker for caching strategy (cache-first for static assets, network-first for API calls with offline fallback). Web app manifest for installability. Background sync for queuing sightings and messages posted offline.

## Priority 2 — Medium Effort, High Impact

### 4. Public Lost & Found Pet Database

| Impact | Ease | Score |
|:---:|:---:|:---:|
| 5 | 2 | **7** |

A searchable, public-facing database of lost and found animals. Members of the public can search by location, species, breed, colour, and date without needing an account. Found-animal reports can be submitted and matched against open cases.

- **Justification:** PawBoost, Pet FBI, and Findpet all centre on this. A public database extends reach far beyond the SAR team and enables the community to self-serve matches. This is the foundation for building a network effect.
- **Approach:** Public search page with filters. Found-pet report form mirroring the lost-pet form. Automated matching suggestions based on species, breed, colour, location proximity, and date range. Case Managers review matches.
- **Considerations:** GDPR implications for publicly visible data — display animal details and approximate location only, never keeper PII.

### 5. Real-Time Location Sharing

| Impact | Ease | Score |
|:---:|:---:|:---:|
| 4 | 3 | **7** |

Show the live GPS position of active searchers on the case map during a search session. Searchers opt in per session from their mobile device.

- **Justification:** CalTopo and D4H both offer this. Critical for field safety (lone worker awareness) and operational coordination (avoiding search overlap, directing searchers to gaps). Particularly important for drone operators who need to maintain separation.
- **Approach:** Browser Geolocation API with periodic position updates via WebSocket or SignalR. Positions displayed on case map with searcher name and role icon. Positions are transient — not persisted after the session ends (privacy by design). Opt-in per session.
- **Dependencies:** Reliable mobile connectivity (ties into PWA/offline work).

### 6. Airspace & Weather Overlays

| Impact | Ease | Score |
|:---:|:---:|:---:|
| 3 | 3 | **6** |

Display airspace restrictions and wind/weather data on the case map for drone operators. PetMap already provides this and it is a key differentiator for drone-focused teams.

- **Justification:** Drone operators need airspace awareness before and during flights. Currently they must switch to a separate tool (PetMap, Drone Assist, NATS). Integrating this into the case map saves context switching.
- **Approach:** Overlay layers on the case map sourced from external APIs:
  - **Airspace:** NATS Drone Assist API or Open AIP data for UK airspace restrictions (CTRs, ATZs, danger areas).
  - **Weather/wind:** Met Office DataHub API or OpenWeatherMap for current conditions and short-term forecast at the case location.
- **Considerations:** API costs and rate limits. Could start with a simple "Open in Drone Assist" link as a low-effort interim.

## Priority 3 — Longer Term

### 7. Cross-Organisation Searcher Community

| Impact | Ease | Score |
|:---:|:---:|:---:|
| 5 | 1 | **6** |

Allow searchers to register independently (not tied to a single organisation) and be discoverable by any organisation operating in their AO. Effectively builds a shared volunteer pool.

- **Justification:** PawBoost's 7M+ Rescue Squad network is its moat. Building a cross-organisation community would be transformative for the pet SAR ecosystem but is a significant product and architectural undertaking.
- **Approach:** Searcher accounts that can be linked to multiple organisations. Organisation-level controls for accepting external searchers (reputation thresholds, approval workflows). Shared reputation across organisations or per-organisation reputation views.
- **Considerations:** Trust and governance model between organisations. Data sharing agreements. Reputation portability.

### 8. Scent Trail Tracking

| Impact | Ease | Score |
|:---:|:---:|:---:|
| 2 | 4 | **6** |

Support logging scent dog search trails as a distinct search type, with scent-specific metadata (dog breed, scent article used, trail confidence, wind direction).

- **Justification:** PetMap supports this. Scent dogs are widely used in UK pet SAR. Currently these would be logged as generic ground search sessions and lose scent-specific context.
- **Approach:** Add a "Scent Trail" search session sub-type with additional metadata fields. Display on case map with a distinct visual style (e.g., dashed line with wind direction arrows).

### 9. Shelter & Animal Services Integration

| Impact | Ease | Score |
|:---:|:---:|:---:|
| 3 | 2 | **5** |

Integrate with local authority stray dog services, animal shelters, and rescue organisations to automatically check whether a reported lost animal has been picked up.

- **Justification:** PawBoost, Pet FBI, and Findpet all partner with shelters. Animals are frequently picked up by dog wardens or handed in to vets/shelters without the SAR team knowing.
- **Approach:** Webhook-based integration where participating shelters can push intake notifications. Alternatively, periodic check against shelter APIs where available. Matched animals surface as high-confidence sightings on the case.
- **Dependencies:** No standard UK shelter API exists. Would likely require individual partnerships.

### 10. Microchip Registry Lookup

| Impact | Ease | Score |
|:---:|:---:|:---:|
| 2 | 3 | **5** |

When an animal is found, allow Case Managers to look up its microchip number against UK registries to confirm identity and locate the registered keeper.

- **Justification:** Findpet offers free microchip registry. Confirming identity via microchip is standard practice when a found animal is taken to a vet. Integrating this into the case workflow saves a manual step.
- **Approach:** Integration with UK microchip databases (e.g., Petlog, PetScanner, Check-a-Chip). Input microchip number, return registered keeper details (subject to registry terms).
- **Considerations:** Registry API access terms and costs. GDPR implications of displaying third-party keeper data.

### 11. Internationalisation (i18n)

| Impact | Ease | Score |
|:---:|:---:|:---:|
| 3 | 2 | **5** |

Multi-language UI and locale-specific formatting. Already identified in [[Functional Specification#8. Future Considerations]].

- **Justification:** Required for expansion beyond English-speaking markets. D4H supports multiple languages. Low immediate impact as initial deployment targets UK.
- **Approach:** Resource file-based string extraction. Locale-aware date, time, and number formatting. Right-to-left (RTL) layout support deferred until needed.

### 12. SMS Notifications

| Impact | Ease | Score |
|:---:|:---:|:---:|
| 2 | 3 | **5** |

Add SMS as a notification channel alongside email, WhatsApp, and in-app push.

- **Justification:** Pet FBI and D4H support SMS. WhatsApp covers the same use case for UK audiences, but SMS has broader reach internationally and does not require the recipient to have WhatsApp installed.
- **Approach:** Twilio or similar SMS gateway integration. Per-message cost model — may need to be configurable per organisation to manage costs.

### 13. Pet Biometric Identification

| Impact | Ease | Score |
|:---:|:---:|:---:|
| 2 | 1 | **3** |

Use nose print recognition or photo-based matching to confirm animal identity. Findpet's nose print technology claims >98.5% accuracy for dogs.

- **Justification:** Emerging technology that could aid in confirming found animals match the reported lost animal, particularly when microchip scanning is not available. However, this is a complex ML/CV integration with limited immediate value to the SAR coordination workflow.
- **Approach:** Integrate with a third-party biometric API (e.g., Findpet's technology if available) rather than building in-house. Capture reference images during case creation; compare against found-animal photos.

### 14. Personnel & Training Records

| Impact | Ease | Score |
|:---:|:---:|:---:|
| 2 | 2 | **4** |

Track searcher qualifications, certifications (e.g., drone operator licence, first aid), and training history.

- **Justification:** D4H's core strength. Useful for tracking drone pilot A2 CofC and GVC certifications, first aid expiry dates, and DBS checks. Lower priority for volunteer animal SAR than for professional emergency services.
- **Approach:** Qualification records linked to searcher profiles. Expiry date tracking with automated reminders. Could gate certain capabilities (e.g., drone flight logging) on valid certification.

## Summary

| # | Feature | Impact | Ease | Score | Category |
|---:|---|:---:|:---:|:---:|---|
| 1 | Lost pet flyer generation | 3 | 5 | **8** | Quick win |
| 2 | Social media integration (Facebook) | 5 | 3 | **8** | Quick win / high impact |
| 3 | Progressive Web App (PWA) | 4 | 4 | **8** | Quick win |
| 4 | Public lost & found database | 5 | 2 | **7** | Medium term |
| 5 | Real-time location sharing | 4 | 3 | **7** | Medium term |
| 6 | Airspace & weather overlays | 3 | 3 | **6** | Medium term |
| 7 | Cross-organisation searcher community | 5 | 1 | **6** | Long term |
| 8 | Scent trail tracking | 2 | 4 | **6** | Medium term |
| 9 | Shelter & animal services integration | 3 | 2 | **5** | Long term |
| 10 | Microchip registry lookup | 2 | 3 | **5** | Long term |
| 11 | Internationalisation (i18n) | 3 | 2 | **5** | Long term |
| 12 | SMS notifications | 2 | 3 | **5** | Long term |
| 13 | Pet biometric identification | 2 | 1 | **3** | Long term |
| 14 | Personnel & training records | 2 | 2 | **4** | Long term |
