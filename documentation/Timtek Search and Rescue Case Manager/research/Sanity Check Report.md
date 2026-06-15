# Timtek SAR — Design Sanity Check

_Consolidated by TARS, 2026-06-14, against [[Functional Specification]], [[Competitive Comparison]], [[Roles]], [[Keepers]], [[Implementation Plan]], and [[Technology Stack]]. Underlying evidence in [[Operational Models]] and [[Workflow and UX Evidence]]._

This is the executive answer to "sanity-check my design against how real lost-pet SAR orgs actually operate." It is opinionated. Where I disagree with what's in your spec, I say so.

---

## Headline verdict

**The design is sound. The single biggest external validator — PulsePoint — also exists; you didn't know it, and once you read that section you'll feel better. The single weakest design choice is the reputation system. Almost everything else is either validated, novel-but-defensible, or fixable with small adjustments.**

Three things matter more than anything else in this report:

1. **AO auto-matching has direct peer-reviewed evidence behind it** (PulsePoint, Stockholm RCT, 14% absolute lift in bystander CPR). Lead with it. It is the most defensible feature in the spec.
2. **Reputation, particularly negative points, is the single design choice most at odds with evidence.** Stack Overflow is the canonical cautionary tale. Make it private, drop public leaderboards, and consider replacing points with outcome-tied badges.
3. **No operational lost-pet SAR org currently uses a purpose-built case manager.** The closest analogue (Lost Dogs Georgia) duct-tapes _eight_ separate consumer SaaS products together. The gap you're targeting is real and unserved.

---

## What's validated by external evidence

| Design choice | Evidence | Confidence |
|---|---|---|
| **AO auto-matching of volunteers to in-scope cases** | PulsePoint RCT (Stockholm: +14% absolute bystander CPR). ~80% of notified pre-EMS bystanders provided CPR. 3,300+ US deployments. Direct analogue with peer-reviewed outcome data. | **Very high** — this is the design choice most strongly validated externally. |
| **Token-based no-account keeper access** | Universal first-24-hour literature describes keepers as in shock and behaviourally compromised. Mandatory password registration on a crying user at 02:00 is hostile design. Token + time-bound access is the right answer. | **High** |
| **WhatsApp-first notifications (UK)** | High open rates, expected UX, supports media and groups out of the box. NGO sector validates 65%+ retention uplifts vs email. PulsePoint's own failure-mode analysis points to email/web push becoming noise within ~3 months. | **High** (with API caveats below) |
| **Role-separated volunteer model (pilot / ground / scent dog / trapper)** | Every operational UK org has this in practice (DSARLD, Sussex SAR, Beauty's Legacy, Eye In The Sky). _None_ have it in software. You'd be the first to model it formally. | **High** |
| **Multi-species support, namespaced case IDs (DS- / HS- / FA- / LA-)** | DSARLD already does this. PetFBI accepts any species. Sussex SAR works dogs only but the surrounding ecosystem is multi-species. | **High** — already validated by your own org. |
| **Defined outcomes including Cold Case** | Most orgs do _not_ formally close cases — Facebook posts simply scroll into oblivion. Doing it properly is a credibility win for reporting, Gift Aid, and future stats publication. | **High** |
| **Public-form-to-case-creation pipeline** | Universal pattern across every operational org. | **High** |
| **Multi-tenant from day one** | Retro-fitting multi-tenancy is the documented road to rewrite or forked codebases. Build the abstractions while it's cheap. _Caveat below._ | **High** |
| **Status taxonomy (Lost / Reunited / Stolen / Stray / Rainbow Bridge)** | Directly mirrors DSARLD's existing public categories. No reason to change it. | **High** |

---

## What's novel — proceed but watch carefully

| Design choice | Why it's novel | What to do |
|---|---|---|
| **The AO concept itself in pet SAR** | No real-world precedent in the domain. Every org allocates by mental map ("Sarah is in Cardiff, send her the Cardiff case"). | First-class onboarding UX with a map widget, default-from-postcode, and one-tap edit. Watch for volunteers ignoring AO and opting into everything. Build the funnel metrics (notified → ack → en route → on scene) from day one — without them you can't tell whether AO is working. |
| **Keeper-facing dashboard with self-service updates and sightings** | No operational org currently does this. Keepers in most orgs report in and then chase by email/DM. | High value if it works; high abandonment risk if the keeper is in panic mode. First screen must be "DO NOT CHASE — here's what to do instead", not a tip in the help section. |
| **Structured sightings triage** | Universal pain point across every org; nobody has it; whoever solves it has a moat. | Build anti-scam friction in (verified account requirement, repeat-offender phone flagging). Triage before notifying searchers — distressed keepers misidentify cats, foxes, plastic bags. |

---

## Where I'd push back on the current spec

These are the changes worth considering before you build past Block 4.

### 1. Reputation/gamification system — the riskiest design choice you've made

The evidence is strongly against the spec as written. Stack Overflow is the canonical case: visible reputation became the goal in itself, high-rep users became gatekeepers, newcomers were dog-piled, intrinsic motivation collapsed, traffic fell off. Mountain Rescue and analogous teams use _private_ training records and response rates, never public scoreboards. The negative-points dimension specifically (−1 for ignoring an in-scope case) has almost no positive precedent and punishes the most common legitimate reasons people don't respond: work, illness, family.

**Recommendation:**

- **Private by default.** No public leaderboards. Visible to the searcher and the ops admin only. This kills 80% of the documented negative emergent behaviour.
- **Drop negative points entirely.** Replace with a coordinator-visible "missed in-scope cases" flag, or simply let positive reputation gently decay if participation drops. The point of a metric is to inform; punishment in volunteer systems just selects for the volunteers who don't mind being punished.
- **Don't gate access on reputation.** Don't let high-rep searchers acquire informal moderator powers; keep ops admin in the hands of org admins.
- **Consider replacing points with badges/streaks tied to outcomes.** "Present at 5 reunions", "1 year active in AO", "10 sectors completed" — harder to game, no negative blast radius, and they tie reward to the thing you actually want (turning up, not signing up).
- **Treat the whole system as a testable hypothesis.** Ship it disabled or coordinator-only at first; observe how volunteers actually behave; iterate before going public.

### 2. The keeper UI is a safeguarding surface and the spec under-recognises it

A keeper who chases their flight-mode dog into traffic on day one is the worst outcome the app can passively enable. Every authoritative first-24-hours resource leads with "DO NOT CHASE" because well-meaning bystanders systematically make cases worse. Token-based access is correct, but the first screen post-intake should be the do/don't checklist with "DO NOT CHASE" front and centre, plus pre-written scripts for bystanders.

**Recommendation:**

- First screen after report submission: do/don't checklist, "DO NOT CHASE" as the lead, "what to say to bystanders" script one tap away.
- Single-column intake form, large fonts, "I can't find this info right now" escape on every field. Do not gate case creation on having a good photo, microchip number, or full address.
- Public case pages: postcode sector (CT1 1) not full postcode (CT1 1FT), never the building. Internal team views can show full detail. Make precision a deliberate output decision, not an accident of data binding.
- Mask the keeper's contact details by default on public pages; route public tips through the case form, not direct phone/email.

### 3. Searcher allocation funnel is invisible in the current spec

VolunteerHub's data: ~40% of new sign-ups never engage with confusing systems, and you can't see them because they never email. Without instrumentation you'll lose your alpha tenant to silent drop-off before you notice.

**Recommendation:**

- First-class funnel metric per case: `in_scope_searchers → notified → acknowledged → en route → on scene → contributing`. Dashboard it for org admins.
- One-tap "can't make this one" reply that converts silence into signal. Doesn't penalise the searcher; tells the coordinator who to ring.
- Time-stamp `case_reported_at` and `pilot_first_acknowledged_at`. Pilot frustration #1 is "late activation" — invisible to coordinators because nobody measures it.
- Per-searcher per-day notification cap. Notification fatigue is the second-biggest killer of these systems after UI confusion.

### 4. Theft cases need to be a workflow variant, not just a status flag

The Pet Detectives (UK, since 1994) treat the missing-vs-stolen distinction as architecturally different: crime ref number capture, police force liaison, _suppress_ the public sightings flyer, contact the microchip database. The Pet Abduction Act 2024 (UK) makes this more important — dogs are no longer treated as inanimate property for theft sentencing, which changes police engagement.

**Recommendation:** Theft cases as a distinct case variant, not a status. Different intake fields, different public-page behaviour, different notification policy, different "what next" guidance to the keeper.

### 5. Pet temperament is the most under-spec'd field

Kat Albrecht's behavioural taxonomy (skittish / social / aloof / xenophobic / hurt) is the single most evidence-backed pet-specific signal in the entire literature. It determines whether a sighting should trigger pursuit, observation-only, or trap-and-camera deployment. The commercial workflow (Drone Pet Recovery et al.) captures it at intake because it drives the search pattern. The Timtek spec currently treats it as free-text.

**Recommendation:** Pet temperament as a first-class case field, driving notification copy, sighting handling rules, and the coordinator's modality decision. **This is the single feature that would most distinguish Timtek SAR from a generic SAR coordination tool.**

### 6. Modality is a first-class concept, not an implicit consequence

Sussex SAR runs drones + ground + scent dogs + humane trapping. The coordinator picks between modalities based on time elapsed, terrain, and pet temperament. Drone and scent-dog do not work simultaneously (drone noise disturbs the scent). Trapping is a multi-day cycle, drone work is bursty. The current `SearchSession` entity will be uncomfortable for trapping.

**Recommendation:**

- `current_modality` on the case, with explicit transitions ("thermal sweep failed → switching to trap-and-camera → deactivating drone alerts → activating trap-handler alerts").
- Distinct `TrapDeployment` entity for trapping (multi-day cycle, daily check, scent refresh, trail camera).
- Coordinator decision-support screen showing inputs side-by-side (time elapsed, terrain, temperament, available resources). _Don't_ try to auto-select modality — coordinator picks; system informs.
- Soft "modality lockout" warning if the coordinator tries to activate drone and scent-dog in the same sector simultaneously.

### 7. Pilot-to-ground handover needs first-class support

Recurring pilot frustration: thermal contact flagged → no ground team available to verify within 20 minutes → contact moves or cools → finding evaporates → pilot resents wasted trip.

**Recommendation:** When a pilot raises a thermal contact, system immediately notifies nearest in-scope ground searchers with one-tap "on my way / can't make it"; surfaces their ETA to the pilot in real time. This single feature would solve the most-cited pilot complaint in the literature.

### 8. The insurance/audit-trail story is implicit and should be explicit

DSARLD provides pilot insurance "during active searches" — this is operationally significant. If Timtek SAR is the system of record for who-flew-what-when, the platform inherits some duty of care. The audit trail needs to be tight and verifiable.

**Recommendation:**

- One-click "active searches as of [date/time]" report per pilot.
- Immutable deployment log (`pilot_assigned_at`, `pilot_acknowledged_at`, `flight_start`, `flight_end`).
- Worth a 30-minute legal scan before live ops — what duty does the platform inherit, what indemnification do you need from each tenant.

### 9. Volunteer expenses ledger (small but valuable)

NSARDA volunteers pay their own fuel; pet SAR is the same; this is rarely surfaced operationally. A per-case expense log helps Gift Aid claims, donation receipts, and volunteer retention.

**Recommendation:** Simple per-case expense capture (fuel/parking/consumables, free-text + receipt photo upload). Roll up per-volunteer per year for tax / donation purposes.

---

## Risks of overengineering

- **Custom mapping tooling.** CalTopo/SARTopo and PetMap already exist. Deep custom mapping is a money pit. Embed a third-party map, store only your polygons/markers.
- **Facebook integration.** Tempting because everyone uses Facebook, but their API and policy environment is hostile and ever-shifting. Read-only "paste a Facebook post URL, parse what you can" import is fine; deep bidirectional integration is a trap.
- **Volunteer matching algorithms.** Real orgs allocate by mental map. An over-clever matching algorithm will be ignored. Keep allocation manual but well-supported (filtered lists by AO match, last-active, skill).
- **In-house GIS.** PostGIS is the right backbone but don't try to replicate what3words, postcode-to-boundary services, OS Maps, or commercial map tile providers. Pay the licence and integrate.

---

## Strategic positioning

**Your natural primary market is deployable volunteer SAR teams** — DSARLD, Sussex SAR, Eye In The Sky, Beauty's Legacy, Lost Dogs Georgia-style affiliates. The federation/franchise model (Eye In The Sky's "Start a Team") means the multi-tenant + per-team scoping you've already specified is essential and correct.

**Sky Paws and pure-marketplace orgs are not really comparable products.** They will always have lower friction than Timtek SAR for casual pilots, because Timtek SAR's value is _operational discipline_, which by definition isn't friction-free. Don't try to match them on signup speed.

**The honest pitch against Facebook is "operational discipline, audit trail, and continuity beyond a founder"** — not feature count. Beauty's Legacy is built around its founder; Lost Dogs of Wisconsin around its founders; DSARLD likely the same. When the founder burns out, the org dies. A multi-tenant tool that lets organisational knowledge live outside founder brains is a defensible value proposition rarely articulated as a sales angle but probably your strongest long-run argument.

**The honest pitch against PetMap is that you do the operations layer it doesn't.** PetMap is a coverage logging tool. DSARLD already use it. Your story is the dispatch, allocation, sightings triage, and case lifecycle that PetMap was never trying to be. Don't try to out-PetMap PetMap.

---

## What I couldn't validate from public sources

These are the things only you / DSARLD can answer. They materially affect design decisions.

- **DSARLD pilot uptake baseline.** "Patchy" is qualitative; what's the number that triggered this project — 10%? 50%? 80%? Tells us what "better" looks like.
- **DSARLD case outcome breakdown** — what proportion ever close cleanly as reunited vs cold? Calibrates the Cold Case outcome's importance.
- **DSARLD insurance specifics** — named-pilot-on-named-case, or umbrella cover? Drives audit-trail strictness.
- **DSARLD theft proportion** — what % of cases are stolen vs strays vs runaways? Drives how much UI weight to give the theft variant.
- **Where in the pipeline are pilots dropping out** — signup, alert noise, alert relevance (wrong geography), or the moment of "do I drive out for this"? Each implies a different design fix.
- **DSARLD's actual internal coordination** — public website is a directory; real dispatch happens somewhere (Facebook Chat? WhatsApp? Admin spreadsheet?). End-to-end mapping needed before live.
- **Has DSARLD considered (or rejected) D4H or similar professional SAR tools?** Knowing why they didn't take them clarifies what Timtek SAR must beat.
- **Is there a friendly second tenant lined up for alpha** (Sussex SAR? Beauty's Legacy? Eye In The Sky?), or does it need to be sourced? Multi-tenant abstractions need a second tenant to validate, ideally early.
- **Has Kat Albrecht's MAR Network material been seen?** Their site was down during research; archive 503'd. The canonical North American pet detective methodology should inform the search-playbook side. Worth a manual look from you.

---

## Suggested next moves

In rough priority order:

1. **Confirm or reject the reputation system redesign** before Block 8 starts. This is the only place where the spec is materially out of line with evidence.
2. **Add pet temperament as a first-class field** in the Block 4 data model (cheap to add now, painful later).
3. **Decide on theft case workflow variant** — full variant, status-only, or out-of-scope-for-v1. Affects Block 4 schema.
4. **Build the funnel metrics dashboard into Blocks 4 and 14** — measurement is what tells you whether AO is working.
5. **Source a friendly second tenant for alpha.** Multi-tenant only works if a second tenant tests it.
6. **30-minute legal scan on insurance / data controller / data processor relationships.** Cheaper now than after the first incident.
7. **Decide on Facebook strategy** — outbound publish-to-Facebook share button is high-leverage and low-cost; deep integration is a trap. Pick the small version.
8. **Document the modality / handover model** as a Block 9–12 design note before implementation — the current spec leaves it implicit.

---

## TL;DR for the impatient

- **Core architecture and most distinctive features are validated by external evidence.** The Timtek SAR concept is right.
- **The reputation system is the one thing that needs material rethink.** Private, drop negative points, consider badges instead.
- **Pet temperament, modality, theft variant, and funnel metrics** are the four small additions that would most improve the design.
- **The market gap is real**: no operational lost-pet SAR org currently uses a purpose-built case manager. The closest analogue runs eight consumer SaaS products in parallel.
- **Your strongest long-run pitch is continuity beyond a founder.** Build for it.

---

_Source documents:_  
[[Operational Models]] — 13 organisations surveyed, cross-cutting observations, implications.  
[[Workflow and UX Evidence]] — 10 evidence questions, PulsePoint validation, GDPR/safeguarding, drone-pilot frustrations.

_Tim's docs cross-referenced:_  
[[Functional Specification]] · [[Competitive Comparison]] · [[Roles]] · [[Keepers]] · [[Implementation Plan]] · [[Technology Stack]] · [[Glossary]] · [[Future Plans]].
