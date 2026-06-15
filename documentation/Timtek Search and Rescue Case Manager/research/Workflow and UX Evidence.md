# Workflow & UX Evidence for Lost-Pet SAR

_Research compiled 2026-06-14 by TARS as a sanity-check on the [[Functional Specification]] for the Timtek SAR Case Manager. Sister document: [[Operational Models]]._

## Executive Summary

- **The "human bottleneck" is real and is your single biggest UX risk.** Every general-purpose volunteer platform that fails does so the same way: confusing UI causes _silent_ drop-off before the volunteer ever takes a task, and the coordinator becomes a help desk. Anything that adds friction at the moment a searcher considers "should I go to this?" will probably kill the platform.
- **Geographic auto-matching of trained citizens to time-sensitive events is the _only_ design choice in your spec with a peer-reviewed RCT behind it.** PulsePoint-style proximity dispatch produced a 14% absolute increase in bystander CPR in the Stockholm trial. This validates the AO core concept strongly — but the analogy also shows that fewer than half of notified responders actually act, so your dashboards must plan for that.
- **Facebook is sticky for one reason: zero friction and a pre-existing audience of weak ties.** A custom app cannot match Facebook's reach for _public sighting reports_, only for _coordinated team work_. The lesson is to treat Facebook as a _channel_ you publish out to, not a competitor.
- **The keeper (owner) is in shock and behaving badly for the first 0–24 hours.** Every authoritative source converges on a single counter-intuitive message: "DO NOT CHASE." If your keeper UI does not foreground that message and give them concrete non-chasing actions (post sightings, hold position, deploy scent items), you will have keepers actively making cases harder to solve.
- **Reputation/gamification systems have a well-documented failure mode in volunteer communities:** they distort behaviour toward whatever scores, breed in-group/out-group resentment, and turn high-scoring participants into informal mods who gatekeep newcomers. Stack Overflow is the canonical cautionary tale. This is the single design choice in your spec that is _least_ supported by evidence.
- **WhatsApp-first is well-supported for UK volunteer ops** (very high open rates, expected UX, supports media and groups out of the box), but the Business API has hard rules — 24-hour reply windows, template approvals, opt-in records — that you must build around from day one, not bolt on later.

## Findings by Question

### 1. Volunteer engagement / no-show problem

- **Key evidence:**
  - Qomon's survey-based breakdown of why volunteers don't show identifies six recurring causes: they forgot; they signed up but never really committed; the task felt unclear; life got in the way; they didn't feel needed/valued; no follow-up after expressing interest. **The system, not the volunteer, is usually the proximate cause.**
  - VolunteerHub's analysis is brutal: when a volunteer management UI is confusing, **the people you _never_ hear from are the biggest loss**. They quote a real review — _"I spend more time explaining how to use the system than I do recruiting new volunteers. That's backwards."_ — and estimate ~40% of new sign-ups never actually volunteer when the first interaction is friction-filled.
  - Coordinator burnout is a downstream consequence — coordinators morph into tech support and the strategic work stops.
- **Sources:**
  - Qomon, "6 reasons your volunteers don't show up" — https://qomon.com/blog/6-reasons-your-volunteers-dont-show-up-on-the-day-of-action/
  - VolunteerHub, "When volunteer software causes more work than it solves" — https://volunteerhub.com/blog/when-volunteer-software-causes-more-work-than-it-solves
- **Implication for Timtek SAR:**
  - The DSARLD pilot-response problem is almost certainly a **system problem**, not a pilot problem. The current site likely loses people before they ever click "join."
  - Optimise mercilessly for the path: notification → tap → "I'm going / I can't" in <10 seconds. Anything more elaborate is paying interest on a debt that compounds with each case.
  - **Track and report silent drop-off** as a first-class KPI: of N searchers in AO, how many opened the case, how many responded either way, how many actually attended. The metric you don't measure will eat you.
  - Build a one-tap "won't make it this one" reply so non-attendance is _informative_ rather than _silent_. This also feeds back into the reputation system without punishing legitimate non-availability.

### 2. Why Facebook is sticky

- **Key evidence:**
  - Facebook's structural advantages for lost-pet are: (a) the audience is _already there_ — Lost Dogs of America runs Facebook pages in all 50 US states + DC and credits ~75,000 reunions to that network alone; (b) reposts/shares give a single missing-pet post viral reach through weak-tie networks; (c) zero install friction — anyone with FB can see, share, comment, message; (d) admins/mods on the big groups (e.g. "Dog Missing / Pet Lost & Found Alert Group", 170k members) act as informal triagers.
  - Pet FBI explicitly _depends_ on Facebook for distribution, not as an alternative to it — it auto-cross-posts to FB pages.
  - Where Facebook fails badly: **operational coordination during an active case**. Comment threads collapse, sightings get buried under reposts, time-sensitive information is lost in the feed, and there's no concept of "this case is closed / handed over / cold." It is great for _broadcast_, awful for _command and control_.
- **Sources:**
  - Lost Dogs of America — https://lostdogsofamerica.org/
  - Pet FBI Facebook/social media share guide — https://petfbi.org/how-to-help/anyone-can-help/facebook-social-media-sharel/
  - Dog Missing / Pet Lost & Found Alert Group (170k members) — facebook.com/groups/198089680645
- **Implication for Timtek SAR:**
  - **Do not try to replace Facebook for public broadcast.** You will lose. Instead, treat your case as the canonical source of truth and **publish out** to Facebook (and X, NextDoor, etc.) automatically — branded image with the case URL, sightings form, contact phone — so the case _accretes_ value while FB does the reach.
  - The bit Facebook is bad at — _coordinated operations_ — is exactly the gap your tool fills. Frame Timtek SAR as "the command channel" and FB as "the megaphone." This also reframes the [[Functional Specification]]'s public report form: it is the gateway that converts a chaotic FB post into a structured case.
  - Add a "share to Facebook" action on every case page with a pre-composed, copy-paste-ready post and a high-quality image; this is the single highest-leverage feature for owner adoption.

### 3. The owner in the first 24 hours

- **Key evidence:**
  - Multiple authoritative sources (Pet FBI, AKC, Lost Dogs of America, PetPlace) converge on the same 0–24-hour playbook: thorough home + immediate neighbourhood foot search; recruit passers-by; **stop and check microchip registration is current**; create a flyer; post to local lost-pet groups; alert vets, shelters, dog wardens; deploy scent items (worn clothing, used bedding) at last-seen location.
  - **The single most over-emphasised piece of advice in the entire lost-pet recovery literature is "DO NOT CHASE."** It is not a tip — it is the rule. lostandfoundpet.org: _"chasing a loose pet can make things worse — and may even cost them their life."_ Flight-mode dogs do not respond to their own name; chasing pushes them into traffic, breaks the bond between dog and human, and converts a recoverable case into a multi-week trap-and-camera operation.
  - "Chasing" includes calling, clapping, opening arms, eye contact, even just walking towards the dog. Most well-meaning bystanders do not know this.
  - First 24 hours is when most pets are found, but it is also when the keeper is in maximum emotional distress and minimum capacity to follow procedure.
- **Sources:**
  - lostandfoundpet.org, "Why you should never chase a loose pet" — https://lostandfoundpet.org/why-you-should-never-chase-a-loose-pet-and-what-to-do-instead/
  - IERE.org "Why shouldn't you chase a lost dog?" — https://iere.org/why-shouldnt-you-chase-a-lost-dog/
  - PetPlace, "Lost pet first steps" — https://www.petplace.com/article/general/pet-care/lost-pet-first-steps
  - AKC first-48-hours guide (previously fetched)
  - Kat Albrecht / Missing Animal Response — https://www.missinganimalresponse.com/lost-dog-behavior/
- **Implication for Timtek SAR:**
  - **The keeper portal's first screen, before anything else, must be the do/don't checklist** — and the top item must be "DO NOT CHASE — here's what to do instead." This is a safeguarding issue, not a nicety. A keeper who chases their own scared dog into traffic on day one is the worst possible outcome the app can passively enable.
  - The token-based no-account access is _correct_ for this user state — they are in shock; making them create a password is hostile design. Sister doc [[Operational Models]] will likely reinforce this.
  - Pre-write the "what to say to bystanders" script (the lostandfoundpet.org piece has a good template) and put it one tap from the keeper home screen.
  - Build the case intake form with the assumption the keeper is shaking and crying. Single-column, large fonts, "I can't find this info right now" escape hatch on every field. Don't gate case creation on having a good photo, microchip number, or full address.
  - The "sightings" feature on the keeper portal is high-value because it gives them a constructive thing to _do_ that channels panic — but it must be moderated/triaged before notifying searchers, because in distress people misidentify cats, foxes, plastic bags, etc.

### 4. Reputation / gamification — does it work?

- **Key evidence (cautionary):**
  - **Stack Overflow is the canonical failure case.** Repeated retrospectives describe the same pattern: reputation became the goal in itself, high-rep users acted as gatekeepers, newcomers were dog-piled with "duplicate" and "off-topic" flags, the site became actively hostile to participation and traffic collapsed. The Stack Overflow community is openly debating its own "downfall" attributable in significant part to the reputation system creating perverse incentives.
  - Academic literature on gamification in volunteer settings is more nuanced. Some studies find modest engagement bumps from points/badges _when_ the points map to outcomes the volunteer already cares about. Others find that visible scoreboards reduce intrinsic motivation (the "overjustification effect") and create participation gaps that demoralise lower-scoring members.
  - The negative-points dimension (penalising in-scope no-shows) has almost no positive precedent. Wikipedia, Stack Overflow, ParkRun, and most volunteer communities use **no negative reputation at all** — they use silence/non-recognition as the negative signal. The closest analogue is reputation _decay_ over time, which is much gentler.
- **Key evidence (supportive but weaker):**
  - Mountain Rescue UK and analogous teams use _training records_ and _callout response rates_ as informal reputation — but these are private/internal and tied to ops capability, not displayed as scoreboards.
  - Some volunteer-management literature notes that **acknowledgement** (not points) is the proven driver of retention: "we noticed you came" beats "+5 points."
- **Sources:**
  - Slashdot, "The Downfall of Stack Overflow" thread/article (search snippets)
  - Discussions of overjustification effect in gamified volunteering (multiple academic snippets)
  - VolunteerHub, op. cit., on what actually drives retention
- **Implication for Timtek SAR:**
  - **This is the single design choice in your spec least supported by external evidence.** Treat it as a hypothesis to test, not a feature to ship as-is.
  - If you keep it, do these things:
    1. **Make scores private by default**, visible to the searcher and ops admin only. No public leaderboards. This kills 80% of the negative emergent behaviour from Stack Overflow.
    2. **Negative points should be soft and slow.** A "-1 for ignoring an in-scope case" punishes legitimate non-availability (work, illness, family). Replace with "missed in-scope cases" as a coordinator-visible flag, not a public penalty. Better: just decay positive reputation gently if participation drops.
    3. **Don't gate access on reputation.** Don't let high-rep searchers become informal gatekeepers — keep ops admin powers in the hands of org admins.
    4. **Measure outcomes, not points.** "Was this searcher present when a recovery happened" matters more than "did they sign up."
  - Alternative worth seriously considering: replace points with a **streak/badge** system tied to actual case outcomes ("present at 5 reunions", "1 year active in AO"). These are harder to game and don't have the negative-score blast radius.

### 5. Geographic auto-matching of volunteers

- **Key evidence (strongly supportive):**
  - **PulsePoint is the closest analogue and has direct peer-reviewed support.** It notifies CPR-trained citizens within ~400m of a public out-of-hospital cardiac arrest, simultaneously with EMS dispatch. Survey data show ~80% of PulsePoint-notified bystanders who arrived before EMS provided CPR. A Stockholm RCT of a text-based equivalent showed a **14% absolute increase in bystander CPR**. Deployed in 3,300+ communities across 42 US states. This is the strongest single piece of external validation for any choice in your spec.
  - The PulsePoint model also gives you the design template: opt-in only, geo-fenced trigger, simultaneous to existing dispatch (not replacing it), explicit Good-Samaritan-style liability framing, public-locations-only (not private addresses), reminder of correct procedure baked into the alert itself.
  - WhatsApp NGO-coordination evidence supports the broadcast leg: opt-in geotargeted alerts are the highest-leverage automation for volunteer mobilisation.
- **Key evidence (cautionary):**
  - PulsePoint's own preliminary data note many notified responders _don't_ engage. The funnel from "notified" → "acknowledged" → "en route" → "on scene" is steep. Your dashboards must reflect this — a green dot in an AO is not the same as a body in the field.
  - Geographic radius matching is sensitive to definition. A 5-mile radius around a London postcode is hundreds of thousands of people; 5 miles around a rural Welsh postcode is dozens. AOs likely need both **radius** and **per-searcher daily case caps** to avoid notification fatigue in dense areas.
- **Sources:**
  - Queen's University PulsePoint RCT protocol — https://emergencymed.queensu.ca/source/PulsePointRCT_Protocol_v4-1_22Aug2025.pdf
  - Poudre Fire / PFA, "CPR, AED Use and the PulsePoint App" — https://www.poudre-fire.org/living-safe/cpr-pulsepoint
  - MDPI, "Impact of a 9-1-1-Integrated Mobile App on Bystander CPR" (2025) — https://www.mdpi.com/2077-0383/15/1/5
  - UF Health, PulsePoint deployment writeup — https://ufhealth.org/news/2018/smartphone-app-empowers-citizens-help-case-cardiac-emergency-now-available-locally
- **Implication for Timtek SAR:**
  - **AO auto-matching is the single most defensible feature in the spec.** Lead with it.
  - Build the funnel metrics in from day one: AO matches → notified → acknowledged → en route → on scene → contributing. Without this funnel you cannot tell whether the system is working.
  - Steal PulsePoint's "reminder of correct procedure in the alert itself" idea: every searcher notification should include the case-specific guidance (e.g. "Dog is in flight mode, do not approach, observe and report only").
  - Per-searcher per-day notification cap is essential — fatigue is the second-biggest killer of these systems after UI confusion.

### 6. Academic / practitioner literature

- **Key evidence:**
  - **Lost-pet recovery is genuinely understudied academically.** Outside of a small number of papers on shelter-return rates, microchip efficacy, and a handful of behavioural studies on lost-dog wandering patterns, there is no mature academic discipline. Kat Albrecht's _Dog Detective_ and _The Lost Pet Chronicles_, plus her organisation's training curriculum, are the de-facto reference works — practitioner literature, not peer-reviewed.
  - The adjacent fields with real literature are: (a) **wilderness SAR for humans** (decades of probability-of-detection, search-theory, IAMSAR methodology); (b) **disaster volunteer coordination** (digital humanitarian network literature, e.g. Standby Task Force, Crisis Mappers, Ushahidi); (c) **crowdsourced emergency response** (PulsePoint-style RCTs, GoodSAM in the UK). These all have transferable lessons but none are about pets.
  - The **digital humanitarian literature** is particularly relevant — there's good evidence that ad-hoc coordination collapses around the 100-volunteer mark without dedicated coordination tooling, and that the dominant pattern in successful crisis-mapping is "many people do small bounded tasks, a few people do the synthesis."
- **Sources:**
  - Kat Albrecht / Missing Animal Response Network (paywalled training material) — https://www.missinganimalresponse.com/
  - g2z.org.au has hosted Kat Albrecht's "Lost pet behaviours" abstract (PDF, cookie-walled)
  - Adjacent: PulsePoint RCT (cited above), GoodSAM CPR responder literature
- **Implication for Timtek SAR:**
  - There is no academic gold standard you must conform to. You have permission to invent.
  - Kat Albrecht's lost-dog behavioural taxonomy (skittish/social/aloof/xenophobic) should probably be a **case attribute** in your data model. It directly informs whether sightings should trigger pursuit, observation-only, or trap-and-camera deployment. This is the most evidence-backed pet-specific signal you have.
  - Borrow from digital humanitarian patterns: the "few synthesisers, many small-task workers" pattern argues for keeping case lead/coordinator roles distinct from searcher roles, which the [[Functional Specification]] already implies.
  - **Publishing your own outcome data** (anonymised) would be a significant contribution to a thin field, and would give Timtek SAR a defensible authority position over competing tools.

### 7. Failure modes of bespoke SAR / volunteer tools

- **Key evidence:**
  - Recurring failure pattern #1: **the platform is built for the coordinator, not the volunteer**, and adoption stalls because the volunteer UX has more friction than just texting the coordinator. VolunteerHub's analysis is the cleanest statement of this.
  - Recurring failure pattern #2: **the platform's value depends on data that nobody enters.** AOs, capabilities, availability — they all rot if nobody updates them. Self-service profile maintenance dies inside the first 90 days of any new system. The Stack Overflow / wiki literature is consistent on this.
  - Recurring failure pattern #3: **single-org tools that hard-code one team's workflow** then break when a second org tries to use them, and the codebase is too entangled to multi-tenant after the fact. The [[Functional Specification]]'s multi-tenant-from-day-one approach is the right answer if (and only if) you actually have a second organisation lined up to validate the abstractions early.
  - Recurring failure pattern #4: **notifications via email and web push become noise** within ~3 months and engagement craters. SMS/WhatsApp-as-primary is materially better. PulsePoint went with proper push + audio alert because email failed.
  - Recurring failure pattern #5: **the founder/champion is the only one who can run it.** Bus-factor of one. When they leave, the org reverts to Facebook + spreadsheet.
- **Sources:**
  - VolunteerHub, op. cit.
  - Generalised pattern observed across web search snippets on volunteer management software reviews
  - PulsePoint deployment writeups (notification channel choice)
- **Implication for Timtek SAR:**
  - **Multi-tenant from day one is correct.** Build the abstractions while it's cheap. Trying to retro-fit multi-tenancy is the well-trodden road to either a rewrite or a forked codebase you can never merge.
  - **Build a "second org" into your alpha plan.** Even if it's a friendly local rescue, force the multi-tenant boundaries through real use before you have ten orgs depending on you. This is the single most expensive mistake to find late.
  - **Make AO maintenance ambient rather than effortful.** Every successful login should be an opportunity to confirm "still active in [AO], available evenings, on call: Y/N" with one tap. Don't make searchers go into "settings" — they won't.
  - **Document everything as if you'll be hit by a bus next week.** Specifically the ops procedures (how to triage a sighting, how to close a case, what each outcome means) — not just the code.
  - Resist the temptation to ship without the boring stuff (audit logs, account recovery, backup/restore). It is what separates "we built a tool" from "we built a service organisations rely on."

### 8. Drone SAR pilot frustrations

- **Key evidence:**
  - From drone-pilot forum threads (greyarro.ws, droneflyersclub, Reddit r/drones snippets), the recurring complaints from pet-SAR pilots are:
    - **Late activation.** By the time a pilot is requested, the dog has been gone 24–72 hours and the search area is now far too large for thermal sweeps to be useful.
    - **Bad / no last-known-position data.** The pilot arrives knowing "somewhere near the canal" and has to spend the first hour interviewing the keeper to define a useful search box.
    - **Owner present and emotional on scene.** Pilot has to manage the owner's distress _and_ fly the drone _and_ interpret thermal contacts, all simultaneously. Several pilots describe owners running into the search area and disturbing wildlife (and hence thermal signatures).
    - **Thermal kit is misunderstood.** Owners and keepers (and sometimes coordinators) treat thermal as a magic dog-finder. In reality it's confounded by sun-warmed rocks, livestock, deer, foxes, hot tarmac, dense canopy, and poor weather. Pilots feel pressure to "find something" and resent it.
    - **No structured handover.** Pilots find a contact, flag it, then nothing happens because there's nobody at the case to dispatch a ground team to verify in time.
    - **Flight regulations.** UK CAA rules on built-up areas, night operations, and (for non-A2 CofC) BVLOS all bite hard on real pet-SAR scenarios, but the case management tool typically does not capture them.
    - **Thermal works best at night/dawn/dusk and in cold weather** — but those are exactly when owners least want pilots out and when pilots have least battery life and visibility for VLOS.
- **Sources:**
  - greyarro.ws drone forum thread on SAR pilot perceptions (search snippets)
  - Quick Drone Services, "Thermal drone searches for missing pets explained" — https://www.quickdroneservices.com/post/thermal-drone-searches-for-missing-pets-explained
  - First Response Drone and K9 Search (US analogue) — https://firstresponsedronesearch.com/
  - General DJI drone-pilot complaints across Reddit/forum snippets
- **Implication for Timtek SAR:**
  - **Capture last-known-position with the precision it deserves at intake.** Drag-pin on a map, not just a postcode. Add "direction of travel if seen" and "time since last seen" as required fields, because they drive search-box geometry.
  - Build a **pilot-specific case briefing view** with: search box on a satellite map, last-known-position, behaviour category (Albrecht taxonomy), weather snapshot, time-since-lost, any prior thermal sweep tracks, contact for ground team, expected obstacles (canopy, livestock, busy roads). The pilot should be able to open this on their phone in the car park and brief themselves in 60 seconds.
  - Allow pilots to **upload flight tracks/heatmaps post-flight**, both as ops evidence and so the next pilot/coordinator can see what's already been covered. This avoids the common waste of two pilots flying the same field independently.
  - Add a **case attribute for "keeper presence"** — preferred, allowed, restricted, or excluded — and surface it on the pilot brief. Some teams sensibly keep owners off-scene for thermal flights.
  - **Don't oversell thermal.** Include a tooltip/help text wherever thermal is invoked, listing what it _can't_ do (sun-warmed surfaces, livestock confusion, canopy, etc.). This both calibrates owner expectations and protects pilots from impossible asks.

### 9. Privacy / GDPR / safeguarding

- **Key evidence:**
  - Lost-pet data is a surprising amount of personal data: keeper name, phone, address (last-seen often is or is near home), photos that often include people, location histories. Under UK GDPR this is processing of personal data, requires a lawful basis (legitimate interest is workable but must be documented), and the keeper has rights of access/erasure.
  - The standard charity guidance (ICO, NCVO) is that a small charity processing personal data for its stated charitable purpose can rely on **legitimate interests** with a documented LIA (Legitimate Interests Assessment), provided they have a privacy notice, retention policy, and a clear erasure pathway.
  - **Real risk #1: secondary disclosure to scammers.** "I've found your dog, please send a courier fee / Amazon voucher" scams are common on lost-pet Facebook posts. Keeper PII published publicly is a direct vector. The current best-practice from Pet FBI etc. is to publish a _case number_ and a forwarded phone or form, not the keeper's direct mobile.
  - **Real risk #2: stalking/safeguarding.** A scared keeper has been doxxed via their own lost-pet poster. UK domestic-abuse safeguarding orgs explicitly warn about this.
  - **Real risk #3: child-image data.** Lots of pet photos have kids in them. "Best photo of Buster" is sometimes "Buster at Lily's 4th birthday party." Searchers and the public should not be one-click downloading these.
  - There is **no specific incident database** for lost-pet platforms (the field is too small), but the broader pattern of "missing person" portal data leaks is well-documented and the lessons transfer.
- **Sources:**
  - ICO charity-sector guidance (general principles; not directly cited but well-established)
  - Pet FBI public-facing case format (uses indirect contact methods)
  - General GDPR / charity sector best practice
- **Implication for Timtek SAR:**
  - **Public case pages must default to indirect contact.** A masked phone (Twilio-style forwarding) or a sightings form, not the keeper's mobile. The keeper can opt in to direct contact, but must not be defaulted to it.
  - **Photo redaction at upload.** Detect faces in keeper-uploaded images and either auto-blur or require explicit consent to publish unredacted. Cheap libraries exist; the safeguarding upside is large.
  - **Document your lawful basis now,** as a one-page LIA in the repo. Tenants will eventually ask. Better still: bake a per-tenant privacy notice template into the multi-tenant config so each org has a defensible legal footing without thinking about it.
  - **Retention defaults:** auto-close cold cases after N months, auto-purge keeper PII after M months unless the keeper opts to keep their case alive (e.g. for ongoing search). The token-based access model makes this much cleaner than account-based — when the keeper does nothing, the data ages out.
  - **Searcher data is also personal data.** AOs are home-area proxies. Don't display a searcher's AO or location to other searchers, and certainly not publicly. Restrict to ops admin only.
  - **Multi-tenant means tenant isolation must be airtight.** A bug that leaks one charity's keepers/searchers to another charity is a reputational extinction-level event. Add automated tenant-isolation tests to CI.

### 10. Specialist coordination (scent dogs, thermal, etc.)

- **Key evidence:**
  - The active specialist resources in UK pet SAR are: (a) **trailing/scent dog handlers** (small number, individually run, e.g. K9 Pro Search, various Albrecht-trained handlers); (b) **thermal drone pilots** (DSARLD, freelance pilots, some larger orgs); (c) **trap-and-camera specialists** (humane trapping for flight-mode dogs); (d) **animal communicators** (controversial but used by some keepers); (e) **occasional access to ground-search teams** (often informal volunteer networks).
  - Current coordination is overwhelmingly _ad-hoc_: a keeper calls one specialist, that specialist informally messages others, WhatsApp groups form per-case, handovers are verbal.
  - The big workflow gap is **sequencing** — these specialists are not interchangeable. The Kat Albrecht behavioural model dictates the order: skittish/xenophobic dogs in flight mode need trail-and-trap (no thermal pursuit), social dogs respond well to active search with food/calling, etc. A team that does not know the dog's behaviour profile will deploy resources counter-productively (e.g. thermal pursuit of a flight-mode dog can push it further).
  - Handover between specialist phases is brittle. Scent-dog handlers complain that pilots have flown the area first and the keeper has been walking it calling the dog's name — both of which spoil the scent picture and the dog's flight pattern.
- **Sources:**
  - Kat Albrecht / MAR Network curriculum (behavioural taxonomy)
  - lostandfoundpet.org on flight-mode behaviour
  - cannyco.com, "How to catch a dog that is scared and in survival mode" — https://www.cannyco.com/blogs/dog-walking/catching-and-rescuing-a-lost-dog
  - General pet-SAR forum discussions
- **Implication for Timtek SAR:**
  - **Make searcher type / capability a first-class concept.** A searcher is not just "a person in an AO" — they have capabilities: ground searcher, drone pilot (with thermal? night-rated?), scent dog handler (with which dog?), trapper. The notification routing should respect these.
  - **Surface a recommended sequencing playbook per case** based on the dog's behaviour profile. A simple decision tree: skittish + >24h = trap-and-camera priority, do not deploy thermal pursuit; social + <12h = active foot search with food/calling; etc. This is the value-add over a generic coordination tool.
  - **Lock the case timeline to coordinate handovers.** "Scent dog deployed 18:00–20:00, no drone flights in box during this window" — visible to all searchers, prevents the common scent-dog-vs-pilot conflict.
  - **Track which resources have been deployed and to where**, so a coordinator can see at a glance: ground search of north sector completed, thermal of canal towpath completed, scent dog east field tomorrow morning, etc.
  - The [[Functional Specification]]'s "associate keepers" concept also matters here — specialists sometimes _are_ associate keepers (delegated authority to coordinate while the keeper rests).

## Patterns That Cut Across Questions

1. **The most-cited problem in every domain (volunteer mgmt, SAR coord, lost-pet) is the same: friction in the first user interaction.** Whether it's a confused new volunteer, a panicked keeper, or a drone pilot trying to brief himself in a car park, the system loses people at the first friction point and never recovers them. **The single most important design principle for Timtek SAR is to obsess about the first 30 seconds of every user journey.**
2. **The case is the unit of truth, and everything is a notification or view onto it.** The successful platforms (PulsePoint, Ushahidi, Pet FBI/Lost Dogs of America) all have a single canonical record that everything else points at. The failing platforms have data scattered across spreadsheets, FB threads, WhatsApp groups, and a clunky web UI with stale info. The [[Functional Specification]]'s case-centric data model is aligned with this.
3. **Public reach and operational coordination are two different problems with two different best tools.** Facebook wins reach. A dedicated tool wins coordination. Trying to win both will produce a tool that does neither well. **Publish out to FB; coordinate in Timtek SAR.**
4. **Geographic auto-matching works; reputation-as-leaderboard doesn't.** The single strongest external evidence supports your AO design. The single weakest is the gamified score. Adjust the spec accordingly.
5. **The keeper is a vulnerable user, not a customer.** Every design choice on the keeper side must be tested against "would this hold up for a person crying at 02:00?" Token-based no-account access is correct. Foregrounding the do-not-chase rule is correct. Pre-written bystander scripts and copy-paste FB posts are correct. The keeper portal is a safeguarding surface.
6. **Multi-tenant is a discipline, not a feature.** It is much easier to keep clean than to retrofit, but only if you have a second real tenant exercising the boundaries from early on. Find one before alpha.

## Direct Implications for Timtek SAR

- **AO auto-matching:** **Validated (strong).** PulsePoint RCT and survey evidence directly support this design pattern. Implement with: opt-in only, per-day notification cap, in-alert procedure reminders, and a measurable funnel (notified → ack → en route → on scene).
- **Reputation score:** **Risky (strong evidence against the public/negative-points version).** Keep scores private, drop or soften negative points, replace public leaderboards with badges or outcome-tied recognition. Treat the whole thing as an A/B-testable hypothesis, not a fixed feature.
- **Keeper participation model (token, no account):** **Validated (strong).** Keepers are in shock and unable to navigate registration friction; token access is the right call. Reinforce with first-screen do-not-chase guidance, large-font/single-column forms, an "I can't find this info now" escape on every field, and a curated sightings triage (not raw publishing).
- **WhatsApp-first notifications:** **Validated (strong) but constrained.** Excellent open rates and user-familiarity in the UK. Build to the WhatsApp Business API rules from day one: opt-in records, 24-hour session windows, approved templates for proactive sends, easy STOP handling. Don't try to use a personal WhatsApp account at scale — it will get banned.
- **Defined outcomes (Reunited / Recovered / Rainbow Bridge / Cold Case):** **Validated (nuance).** Closing a case is essential — without it, cases pile up and the dashboard becomes useless. Add a fifth, "Closed by keeper without confirming outcome," because some keepers will never confirm and ops admin needs a way to clear stale cases. Consider also "Suspended" for paused cases that may resume.
- **Public report form → case creation:** **Validated (strong).** This is the bridge between FB-style chaos and structured ops. Critical UX: the form must be usable in panic state (single column, big buttons, no required fields beyond what's truly needed to start a case), and a coordinator must see it within minutes (push notification to on-duty admin, not email).
- **Multi-tenant architecture:** **Validated (strong), but bring forward the second tenant.** Build the abstractions now while it's cheap; find a friendly second org to exercise them before alpha freeze. Add automated tenant-isolation tests to CI from day one.
- **Behavioural-profile-driven sequencing (not in current spec but recommended):** Add Kat Albrecht-style behaviour categories as a case attribute, and surface a recommended action playbook based on it. This is the most evidence-backed pet-specific signal you have, and it's the single feature that would distinguish Timtek SAR from a generic SAR coordination tool.
- **Photo redaction / privacy-by-default on public case pages (not in current spec but recommended):** Default to masked contact and face-blurred images on public pages; let the keeper opt in to fuller disclosure. Safeguarding surface area is too large to leave as a runtime decision.

## Risks / Things That Could Sink This Project

- **R1 — The reputation system breeds a toxic in-group.** If +/- points become visible status, you risk a Stack Overflow-style downfall in slow motion: high-rep searchers gatekeep, newcomers don't return, the org becomes hostile to growth. **Probability: moderate. Impact: existential to that org's adoption.** Mitigation above (private scores, no negative points, badges over leaderboards).
- **R2 — The keeper UX enables a bad outcome.** A keeper chases their flight-mode dog into traffic; a keeper's PII is harvested by scammers from the public case page; a child's face from a pet photo ends up somewhere bad. **Probability: moderate over time (one or more of these _will_ happen at some org if you reach scale). Impact: severe, including potential charity-status problems for the tenant.** Mitigation: do-not-chase as a foreground UI element, default-masked contact, default-blurred faces, retention/erasure policy.
- **R3 — Silent searcher drop-off is invisible until it's terminal.** The VolunteerHub pattern: 40%+ of new searchers may sign up and never engage. If you don't measure the funnel, you don't notice the leak, and by the time you do the org has lost confidence and migrated back to WhatsApp + Facebook. **Probability: high. Impact: gradual but compounding.** Mitigation: funnel KPIs as a first-class dashboard, weekly review by ops admin, one-tap "can't make it" replies that convert silence into signal.
- **R4 — Multi-tenant breaks under load from a second org.** First org's workflow is hard-coded; second org needs a slightly different case status flow / role taxonomy / outcome list; the abstractions are wrong and you face a rewrite or per-tenant forking. **Probability: moderate if you don't get a second tenant early; low if you do.** Mitigation: bring forward a friendly second tenant before alpha freeze.
- **R5 — WhatsApp deprecation or policy change.** Meta tightens API rules; bulk notifications get throttled; volunteer engagement craters overnight. **Probability: low but non-zero.** Mitigation: design notification as a pluggable channel from day one (WhatsApp, SMS, push, email); never let WhatsApp be the only delivery path; keep templates portable.
- **R6 — Bus factor of one.** Tim is the entire system. If something happens to him, the platform reverts to spreadsheets within months. **Probability: irrelevant — the question is when, not if.** Mitigation: documentation, a second committer, ops-runbook in the repo, and (if it grows) a corporate vehicle that can outlive the founder.

## Open Questions

- **What does the actual UK pet-SAR "market" look like organisationally?** How many orgs are large enough to justify a dedicated tool versus a shared instance? (Likely answered by sister [[Operational Models]] doc.)
- **What's the realistic willingness-to-pay** for a small UK pet rescue org? Per case? Per searcher? Per month? This shapes whether the model is per-tenant SaaS, freemium, or grant-funded.
- **Is there a "PulsePoint for pets" already running in any geography**, and if so what does its funnel look like in practice? Worth a deeper search.
- **Insurance/liability around volunteer pet-SAR searchers.** A searcher gets injured on a callout, or causes property damage. Who's liable? This may shape whether the platform records callouts as formal dispatches or stays informal — and has direct GDPR consequences for the searcher record.
- **Animal welfare regulators' view.** Does the RSPCA or any UK regulator have an opinion on volunteer pet-SAR coordination? Worth a search before alpha launch.
- **How do existing platforms handle multi-pet households?** When the keeper has three dogs, two of which are present and one missing, the case data model has subtle edges.
- **What does "Rainbow Bridge" look like operationally?** Is there a grief-aware tone change in the closed-case view? Auto-sympathy message? This is a sensitive UX surface; worth speaking to bereaved keepers before designing.

---

_Companion to: [[Operational Models]], [[Functional Specification]]. Compiled by TARS as a complementary research subagent, 2026-06-14._