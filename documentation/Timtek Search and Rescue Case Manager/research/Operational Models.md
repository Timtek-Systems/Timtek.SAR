# Operational Models in Lost Pet SAR

_Research compiled 2026-06-14 by TARS for the Timtek SAR sanity-check._

This document surveys how real-world lost-pet search and rescue organisations actually operate, to sanity-check the design of the [[Functional Specification|Timtek SAR Case Manager]]. The seven competitors already analysed in [[Competitive Comparison]] (PetMap, PawBoost, Pet FBI, Findpet, D4H, CalTopo/SARTopo, Facebook Groups) are deliberately not repeated here. The focus is on operational practice: who runs cases, how they coordinate, how they allocate searchers, and where they hurt.

---

## Executive Summary

- **The market splits cleanly into four operational archetypes**: (1) database-and-flyer networks (PetFBI / Lost Dogs of America), (2) hands-on volunteer SAR teams with deployable assets (DSARLD, Sussex SAR, Eye In The Sky, Beauty's Legacy), (3) pure broadcast notification marketplaces (Sky Paws, Drone Animal Rescue US), and (4) paid commercial pet detectives (The Pet Detectives, Elite Deer Recovery sideline). Timtek SAR sits in archetype (2), which is by far the most operationally complex and the worst served by existing software.
- **Almost no one uses a purpose-built case manager.** Even the most codified org found (Lost Dogs Georgia) coordinates across **eight separate tools**: Gmail, G-Voice, G-Calendar, PetFBI's database, the Lost Dogs of America Code of Conduct PDF, a Flyer Maker forum, Facebook Page admin, and a private Facebook Chat. This is the gap Timtek is targeting.
- **Searcher allocation is almost universally manual** — usually by an admin/coordinator who eyeballs a Facebook post or a Google Maps pin against their mental model of who lives nearby. No org found uses a formal [[Glossary#AO|Area of Operation]] model. This validates Timtek's AO design as genuinely novel for this domain.
- **Public/keeper engagement is one-way in most orgs.** Owners report in, receive an action plan PDF or a Facebook post link, and then have to chase updates by email or DM. Only DSARLD's own website surfaces per-case status to the public. Timtek's owner-facing dashboard concept (see [[Functional Specification]] §6) has no real precedent.
- **Sightings triage is the universal pain point.** Every org that runs active searches reports being overwhelmed by well-intentioned public sightings, scam attempts, and duplicate reports. No org found has a structured sightings workflow beyond "post on Facebook, admin reads comments". This is a high-value area for Timtek to do better.
- **Cases rarely formally close.** Stats orgs publish (DSARLD's 7,556 reunions, Lost Dogs of America's 75,000+) come from manual updates by owners or admins. There is no industry-standard "cold case" concept — most orgs let Facebook posts simply scroll into oblivion. Timtek's explicit [[Glossary#Cold Case|Cold Case]] outcome would be unusually disciplined.
- **Volunteer churn is the existential risk** every org talks around but few address directly. Most orgs lean on a founder or 2–3 core admins who personally triage every case. When those people burn out, the org dies. A system that lets organisational knowledge live outside founder brains is a defensible value proposition.

---

## Organisations Surveyed

### Drone SAR For Lost Dogs UK (DSARLD) — United Kingdom

- **URL:** [dronesarforlostdogs.co.uk](https://dronesarforlostdogs.co.uk/)
- **Model:** Tim's own org. UK's largest free, volunteer drone SAR for animals. Founded 22 July 2017 by Graham Burton in South Wales. Now operates UK-wide.
- **Volunteer structure:** Role-based, codified. Four published volunteer roles including **Drone Pilot** (thermal/standard), **Ground Searcher**, plus two coordinator-style roles. Volunteers may hold multiple roles. The volunteer handbook is explicitly "under constant review" and structured around: Volunteer Roles / How Alerts Work / On a Search / Stop, Drop, Think / What to Bring / Code of Conduct / Quick Reference / FAQ. The published ethos is "No obligation, no pressure" — pilots opt in to specific cases, not to a roster.
- **Case intake:** Keeper-facing "Report a Lost Dog" form on the website captures photo, postcode, description. Prominent scam warning ("we will never charge"). Currently extended to dogs, **horses (HS-)**, **farm animals (FA-)**, and **other large animals (LA-)** — case ID prefixes are already namespaced.
- **Tooling:** A custom website acting primarily as a **public-facing case directory**. Status taxonomy is **Lost / Reunited / Stolen / Stray / Rainbow Bridge**. The site cross-references PetMap as a coverage logging tool. The actual coordination layer — who's flying, who's been allocated, who's en route — is **not** in the website; it lives on Facebook and (per Tim) suffers patchy pilot uptake and poor response rates. This is precisely the gap [[Functional Specification|Timtek SAR]] is built to close.
- **Workflow:** Keeper submits form → admin posts to relevant Facebook groups and emails pilots → pilots respond to admin or directly to keeper → manual update to the public directory when status changes.
- **Stats (homepage, observed 2026-06-14):** 7,556 dogs reunited since 2017, 102 currently active searches, **58 hour average found time**. The 58-hour metric is unusually concrete and worth treating as a design target for Timtek's reporting layer.
- **Insurance:** "Insurance provided for drone pilots during active searches" — this is operationally significant. It implies the org needs to know *which* pilot is on *which* case at *which* time, or insurance is unenforceable. This is an under-stated requirement for the Timtek deployment/dispatch model.
- **Notable:** Has already codified what most orgs leave informal — multi-species support, namespaced case IDs, public status surfacing, role separation between pilot and ground searcher. The bottleneck is coordination, not data model.
- **Sources:** [About](https://dronesarforlostdogs.co.uk/about), [How It Works](https://dronesarforlostdogs.co.uk/how-it-works), [Volunteer](https://dronesarforlostdogs.co.uk/volunteer).

### Sussex SAR For Lost Dogs CIC — United Kingdom

- **URL:** [sussexsarforlostdogs.org.uk](https://sussexsarforlostdogs.org.uk/)
- **Model:** Regional UK SAR CIC. **Broader service mix than DSARLD**: drones + ground searchers + **scent dogs** + **humane trapping**. Partners with Sussex Crimewatch, council dog wardens, and local vets.
- **Workflow:** The same role-and-deploy pattern as DSARLD but with a multi-modality response — the coordinator's job is harder because they're picking between modalities (deploy drone? humane trap? scent dog?) based on case profile.
- **Notable:** The multi-modality model is a strong argument for Timtek's role/skill model needing to handle non-drone disciplines from day one. If Timtek can model "this case needs a scent dog handler and a trapping team", it instantly addresses Sussex-style orgs as well as DSARLD-style. See [[Roles]].
- **Sources:** [About Us](https://sussexsarforlostdogs.org.uk/about-us).

### Eye In The Sky Drones — United Kingdom

- **URL:** [eyeinskydrones.com](https://www.eyeinskydrones.com/)
- **Model:** Not-for-profit drone-assisted lost dog search service. Dorset-based with a stated nationwide remit. Heavy YouTube presence — multi-day search documentaries, individual case follow-ups, public success/failure storytelling.
- **Volunteer structure:** Has a **"Start a Team"** call-to-action on the volunteer page — i.e. they franchise teams by region rather than running everything centrally. This is structurally different from DSARLD's "single org, many volunteers" model.
- **Tooling:** Wix-style website + YouTube + presumably Facebook for live coordination (no public coordination tooling visible).
- **Notable:** The franchise/team-of-teams model has implications for Timtek multi-tenancy. If Timtek is to be sellable to orgs like Eye In The Sky, the [[Roles|Case Manager]] role probably needs a layer above it — a "Team Lead" who only sees their team's cases.
- **Sources:** [Home page](https://www.eyeinskydrones.com/).

### Sky Paws — United Kingdom

- **URL:** [skypaws.uk](https://skypaws.uk/)
- **Model:** **Pure broadcast notification marketplace.** Not a SAR organisation in the operational sense — Sky Paws never runs a search. They aggregate drone pilot signups, and when a keeper reports a lost dog, the report is fanned out by email to all registered pilots in the area. Pilots then contact the keeper directly. No central coordination. No case management. No sightings handling.
- **Volunteer structure:** Open — any drone pilot can register. Sky Paws explicitly offers a separate "SAR organisation registration" so org coordinators can also subscribe to alerts.
- **Tooling:** Forms-based web app on Microsoft Azure UK. Reports auto-deleted after 30 days (privacy by retention limit).
- **Notable:** The pure-marketplace model is what Timtek competes *against* in the "do nothing custom" base case. Sky Paws demonstrates that lightweight, free, no-coordination alerting can run sustainably — but the implicit cost is offloaded to keepers (who now have to manage a fan-out of pilot responses themselves) and to pilots (who have no shared situational awareness). The Timtek value proposition over Sky Paws is *operational discipline* — and the design should be honest that Sky Paws will always have the lower friction for casual pilots.
- **Sources:** [Home page](https://skypaws.uk/), [Drone Pilot Registration](https://skypaws.uk/drone-pilot-registration/).

### Lost Dogs of America (and state affiliates) — United States

- **URL:** [lostdogsofamerica.org](https://lostdogsofamerica.org/)
- **Model:** **Federated network of 50+ state-level volunteer organisations**, each running a state-specific Facebook page. Founded by Lost Dogs of Wisconsin + Lost Dogs Illinois. Claims 75,000+ dogs recovered network-wide. Partnered with **Pet FBI** as the shared database backbone.
- **Volunteer structure:** State-level. Lost Dogs Georgia's published **New Volunteer Checklist** is the single best documented onboarding ritual found in this survey. New volunteers receive, in order:
    1. Sign the **Lost Dogs of America Code of Conduct**.
    2. Add to the state Flyer Maker forum.
    3. Create a PetFBI account using their own email.
    4. Add to the **PetFBI Admin Panel**.
    5. Add to the state Lost Dogs Facebook Page admins.
    6. After training, add to the private Facebook Chat.
    7. Add to the state volunteer webpage.
    8. Add to **Gmail, G-Voice, and G-Calendar** for the org.
- **Tooling:** Eight separate platforms duct-taped together. The G-Voice number is the org's public hotline; G-Calendar tracks volunteer shifts; PetFBI is the case database; Facebook Page is publishing; Facebook Chat is dispatch; Flyer Maker is asset production.
- **Workflow:** Keeper submits to PetFBI → state admin picks it up and posts to state Facebook page → volunteers post flyers, field public tips, coordinate via Facebook Chat → owner updates PetFBI when reunited.
- **Notable:** This is **the closest real-world analogue to a SAR case manager that currently exists** — and it's a manually-orchestrated bundle of consumer SaaS. The 8-step onboarding checklist is essentially what an admin of a Timtek tenant would replace with a single "invite volunteer" flow. The Code of Conduct artefact is also notable; Timtek may want a per-tenant onboarding artefact attachment.
- **Sources:** [Home](https://lostdogsofamerica.org/), [Lost Dogs Georgia volunteer checklist](https://www.lostdogsgeorgia.org/new-volunteer-checklist.html), [Lost Dogs Florida](https://www.lostdogsflorida.org/).

### Pet FBI — United States / Canada

- **URL:** [petfbi.org](https://petfbi.org/)
- **Model:** Volunteer-run nonprofit national lost/found pet database, partner-of-record to Lost Dogs of America. Free.
- **New 2026 feature observed:** **Automatic Report Matching Dashboard** — surfaces lost and found reports that match yours by location, date, and animal type. This is a significant move from PetFBI from "database of reports" to "matching engine". Timtek's sightings module should be aware that even the lowest-common-denominator database is now doing this.
- **Volunteer structure:** "Special Agents" — public volunteers who receive alerts for lost/found pets in their area. Pure broadcast model on top of the database.
- **Notable:** PetFBI is doing what Timtek would call "matching" at the database level. Timtek can add the *operational dispatch* layer that PetFBI doesn't — the question for owner of a case is no longer "is there a match in the database?" but "is anyone going to *do* anything about it?"
- **Sources:** [Pet FBI home](https://petfbi.org/).

### Beauty's Legacy — United Kingdom

- **URL:** [beautyslegacy.co.uk](https://www.beautyslegacy.co.uk/)
- **Model:** UK volunteer-led charity (Charity Commission no. 1194186, registered 2021; founded April 2016 as NPO by Lisa Dean after her cat Beauty died). Free missing pet search service. Also runs a pet theft prevention awareness arm.
- **Workflow:** **Hub-and-spoke around the founder.** Lisa Dean personally takes the call, builds the "structured plan of action", and then allocates the case to volunteers. "Reliable resources and manpower" are pushed at the case, not pulled by the keeper.
- **Notable:** This is the small-charity pattern. **Founder is a single point of failure.** Timtek's design choice of letting any [[Roles|Case Manager]] handle any case within their org is the right antidote — but the system also needs to support the founder-driven workflow as a starting state, then progressively distribute load as the org matures.
- **Sources:** Web search results (the Beauty's Legacy Volunteers page itself returned only Wix CSS, no body content — fetch failure).

### National Search and Rescue Dog Association (NSARDA) — United Kingdom

- **URL:** [nsarda.org.uk](https://nsarda.org.uk/)
- **Model:** Umbrella body for UK volunteer **missing person** (not pet) search dog teams. Included here because it's the established UK SAR coordination model that pet SAR is often compared to.
- **Volunteer structure:** Owner-handlers with their own qualified dogs. Three discipline streams: **Air Scenting**, **Water**, **Trailing**. Member associations across UK, Ireland, Isle of Man. Tasked by police (e.g. An Garda Síochána).
- **Tooling:** Public "Call Outs" log with date, tasking agency, location, dog teams deployed, and named handlers. This is **public operational logging** at a level pet SAR orgs don't currently do.
- **Notable:**
    - The "Call Out" log is a great pattern Timtek could borrow for its public reporting layer — concise, public, but redacted enough to be safe.
    - NSARDA is explicit that **every volunteer is unpaid and pays their own expenses including fuel**. Pet SAR orgs assume this implicitly. A Timtek tenant could usefully surface volunteer expenses for tax/donation purposes; this is a small but real omission in the current spec.
- **Sources:** [NSARDA home](https://nsarda.org.uk/), [Call Outs](https://nsarda.org.uk/call-outs/).

### Missing Animal Search Dog Network (MASDN) — United States

- **URL:** found via web search; main site intermittent
- **Model:** **Pure directory / quality-mark model.** A vetted list of search dog handlers in the US. Handlers are *invited* (not free signup); requirement is **5+ years experience**.
- **Notable:** No coordination layer at all. Just a curated list. The trust signal *is* the product. Timtek does not directly compete here, but the model is a useful reminder that "verified pilot" badges in the system could be valuable in themselves — keepers will pay attention to who's accredited even without any direct dispatch involvement.
- **Sources:** web search snippets.

### Drone Animal Rescue (US) — United States

- **URL:** drone-animal-rescue.org and affiliates
- **Model:** 275+ pilot network. Free for pilots to join. Funded partly by selling drone kits and training packages — i.e. **marketplace + merch**.
- **Volunteer structure:** Open pilot network, no operational case management.
- **Notable:** The merch-funded model is the closest US analogue to Sky Paws. Validates that lightweight broadcast networks can scale to hundreds of pilots — but again with no case-level discipline.
- **Sources:** web search snippets.

### Drone Pet Recovery & Elite Deer Recovery (US) — United States

- **Model:** Commercial paid drone services. Drone Pet Recovery's "How It Works" page is interesting because it **explicitly walks owners through self-help first** (food, scented clothing, photo prep) before they pay for deployment. Elite Deer Recovery sells pet recovery as a sideline to their main deer recovery business.
- **Notable:** Even paid commercial services lead with owner self-help education. This validates the [[Functional Specification|Timtek]] approach of giving keepers a structured action plan from the moment they report.
- **Sources:** web search snippets.

### The Pet Detectives — United Kingdom

- **URL:** [thepetdetectives.com](https://thepetdetectives.com/)
- **Model:** UK private detective agency (since 1994) specialising in recovery of **stolen** dogs. Paid service.
- **Notable:** The hard distinction they draw — **"is this a runaway or a theft?"** — is operationally important and currently sits outside DSARLD's main flow. DSARLD's status taxonomy includes Stolen, but theft cases need fundamentally different workflows (police liaison, crime ref number, no public sightings flyer). Timtek's case manager should probably treat **theft cases as a distinct workflow variant**, not just a different status.
- **Sources:** [Missing & Stolen Dogs](https://thepetdetectives.com/our_services/Missing_and_Stolen_Dogs).

### Stolen And Missing Pets Alliance (SAMPA) — United Kingdom

- **URL:** [stolenandmissingpetsalliance.co.uk](https://www.stolenandmissingpetsalliance.co.uk/)
- **Model:** Campaign/advocacy. Three aims: **Prevention, guidance to victims, campaign for tougher laws.** Runs Dog Theft Awareness Day (14 March, since ~2018).
- **Relevance:** The UK **Pet Abduction Act came into force 24 August 2024** (per Royal Kennel Club campaign page) — dogs are no longer treated as inanimate property for theft sentencing. This changes the police liaison side of stolen-dog cases and is worth referencing in any [[Glossary|theft-case workflow]] in Timtek.
- **Sources:** [About SAMPA](https://www.stolenandmissingpetsalliance.co.uk/about-sampa/), [RKC Paw and Order](https://www.royalkennelclub.com/about-us/about-the-rkc/campaigns/dog-thefts).

### Helping Lost Pets (HeLP) — Canada / North America

- **Model:** Map-based platform partnered with Lost Dogs of America. The about page now redirects to PetFBI — HeLP appears to have been absorbed or rebranded. Historically described as "the only map-based website that facilitates reporting of lost and found pets from the public, shelters, rescues, veterinarians and other pet-related businesses."
- **Notable:** The redirect to PetFBI is itself a finding — the **map-based model lost its independent identity** and got folded into a database-first product. This is a useful data point for Timtek's design: pure mapping without case management does not sustain a standalone product.
- **Sources:** redirect from helpinglostpets.com/about to petfbi.org.

### Also noted (one-line)

- **DogLost UK** ([doglost.co.uk](https://www.doglost.co.uk/)) — Long-standing UK lost-dog database. FAQ extraction failed twice; based on the home reference from the SAMPA results, still operating as a "register your missing pet" database.
- **Lost Dogs UK** (lostdogsuk.co.uk) — Associated with Dial A Pest / SDK Dog Services; found-dog focus. Smaller, regional.
- **Lost Paws UK** — Not a SAR org; this name is taken by a **microchip database vendor**. Worth knowing if Timtek ever picks a similar brand.
- **Lost Dog Trapping Team** (UK) — Facebook-only volunteer trapping specialists. Pure social-coordination model.
- **Scentdog.co.uk** — UK scent-dog training/equipment supplier, not an operational SAR org.
- **Missing Animal Response Network / Kat Albrecht** (US) — Training network for pet detectives. Main site (missinganimalresponse.com) was down (ERR_CONNECTION_CLOSED) and archive.org returned 503 during research. Worth Tim revisiting directly.
- **Lost.ca** — Canada's largest pet lost-and-found community (database, not deployment).
- **NPRA / state pet recovery associations** — Many US states have one. All appear database-and-Facebook in style.
- **Royal Kennel Club "Paw and Order" campaign** — UK advocacy on theft sentencing; relevant context for stolen-dog case workflow.

---

## Cross-Cutting Observations

1. **Facebook is the default coordination layer.** Every operational org found (DSARLD, Sussex, Beauty's Legacy, Lost Dogs of America affiliates, Eye In The Sky, the trapping teams) relies on Facebook Pages or Facebook Chat as the live coordination surface. This is universal because *that is where keepers post when their dog goes missing*. Any case manager that doesn't have a story for "intake from Facebook posts" will fight the tide.
2. **Custom websites are mostly public directories, not coordination tools.** DSARLD's own site, Lost Dogs Florida, Lost Dogs Arizona — all are essentially shop fronts and case directories. The actual moment-to-moment dispatch happens elsewhere (Facebook Chat, email, phone). The Timtek pitch — that the website *is* the coordination tool — is genuinely differentiated.
3. **The org/admin/volunteer hierarchy is roughly universal.** Every team-based org has 1–4 admin/coordinator people and a wider pool of opt-in volunteers. Admins triage, allocate, and update; volunteers execute and report back. Timtek's three-tier [[Roles]] model (Org Admin, Case Manager, Volunteer) maps cleanly onto observed practice.
4. **No one models geography formally.** Allocation is by mental map — "Sarah is in Cardiff so send her the Cardiff case." The closest formal structure observed is the US state-pages model (geography by Facebook page). Timtek's [[Glossary#AO|Area of Operation]] concept has no real-world precedent in pet SAR; that's both an opportunity and a usability risk (volunteers may not understand it without onboarding).
5. **Codes of Conduct and scam warnings are everywhere.** Both DSARLD and Lost Dogs of America make scam awareness front and centre. The lost-pet scene attracts predatory "finders" who demand payment for fake sightings. Any system that connects keepers to public sightings *must* have anti-scam friction baked in (e.g. requiring sightings to be attached to verified accounts, surfacing repeat-offender phone numbers).
6. **Multi-species support is more common than expected.** DSARLD covers dogs, horses, farm animals, and other large animals using namespaced case IDs (DS-, HS-, FA-, LA-). Sussex SAR covers all dogs. PetFBI covers any species. The data model needs to support more than just dogs from day one if Timtek is to compete realistically.
7. **Insurance liability is rarely discussed publicly but is operationally critical.** DSARLD's mention of pilot insurance during active searches implies the org tracks deployments. This is a feature dressed as a compliance requirement: who was deployed, where, when, for how long.
8. **Most orgs are founder-fragile.** Beauty's Legacy, Lost Dogs of Wisconsin, even DSARLD itself — the institutional memory lives in a small number of people. A tenant-aware case manager with stored procedure templates is a way to outlive any one founder. This is rarely articulated as a sales angle but is the strongest long-run argument for the product.

---

## Implications for Timtek SAR

- **Validated by evidence:**
    - Role-separated [[Roles|volunteer model]] with skill tags (drone pilot vs ground vs scent dog vs trapper) — every operational org has this in practice, none have it in software.
    - Multi-species case handling with namespaced IDs — DSARLD already does this, Timtek should match from day one.
    - Public case directory with status taxonomy (Lost / Reunited / Stolen / Stray / Rainbow Bridge) — directly mirrors DSARLD's existing public categories.
    - Keeper-facing intake form with structured fields — universal pattern.
    - Formal case outcome including a [[Glossary#Cold Case|Cold Case]] state — no competitor does this, and it's a credibility win for ops reporting.
- **Novel — proceed with care:**
    - The [[Glossary#AO|Area of Operation]] model: no real-world precedent. Strong on paper but Timtek will need first-class onboarding UX (a map widget, suggested defaults from postcode, easy edit). Watch for volunteers ignoring it and just opting in to everything.
    - Owner/keeper dashboard with self-service updates and sightings visibility: no real-world precedent. High value if it works; high abandonment risk if the keeper is in panic mode.
    - Structured sightings triage: universal pain point. This is one of the most valuable features Timtek can build well.
- **Missing or under-served in the current spec:**
    - **Theft case workflow as a distinct variant.** The Pet Abduction Act (UK, 2024) plus the Pet Detectives evidence say theft cases differ structurally: crime ref number capture, police force liaison, *suppress* public sightings flyer, contact microchip database. Worth elevating beyond just a status flag.
    - **Pilot insurance ledger.** If DSARLD genuinely insures pilots during deployments, the system must record *who was deployed on what case at what time* as a first-class audit artefact. This should be one click off any case page.
    - **Volunteer expense capture.** NSARDA volunteers pay their own fuel. Pet SAR is the same. A simple per-case expense log helps Gift Aid claims, donation receipts, and volunteer retention.
    - **Code of Conduct artefact attachment per tenant.** Lost Dogs of America make signing the CoC the first onboarding step. Timtek's invite flow could attach a CoC PDF that the new volunteer must e-sign before activation.
    - **Public "Call Outs" log.** NSARDA-style public deployment record (date, location, teams deployed, outcome) is a low-effort credibility-building feature.
- **Risk of overengineering:**
    - **Custom mapping tooling.** CalTopo/SARTopo and PetMap already exist; deep custom mapping is a money pit. Embedding a third-party map and storing only the polygon/markers is the right scope.
    - **Facebook integration.** Tempting because everyone uses Facebook — but Facebook's API and policy environment is hostile and ever-shifting. A read-only "paste a Facebook post URL, parse what you can" import is fine; deep bidirectional integration is a trap.
    - **Volunteer matching algorithms.** Real orgs allocate by mental map; an over-clever matching algorithm will be ignored. Keep allocation manual but well-supported (filtered lists by AO match, last-active, skill).
- **Strategic positioning:**
    - Timtek's natural primary market is **archetype 2 orgs (deployable volunteer SAR teams)** — DSARLD, Sussex, Eye In The Sky, Beauty's Legacy, Lost Dogs Georgia. The federation/franchise model means multi-tenant + per-team scoping is essential.
    - Sky Paws and pure-marketplace orgs are competitors only at the keeper-acquisition layer. They are not really comparable products.
    - The honest pitch against Facebook is *operational discipline, audit trail, and continuity beyond a founder* — not feature count.

---

## Open Questions

- **What does DSARLD's current internal coordination actually look like in detail?** The public website is a directory; the *real* dispatch happens somewhere — Facebook Chat? WhatsApp? An admin spreadsheet? Tim is the right person to map this end-to-end before Timtek goes live.
- **What is the failure mode of the current DSARLD pilot signup?** Tim says pilot uptake has been patchy. Is the friction in (a) signup, (b) alert noise, (c) alert relevance (wrong geography), or (d) the moment of "do I drive out for this"? Each implies a different design fix.
- **Insurance policy specifics.** Does the DSARLD pilot insurance require named-pilot-on-named-case records, or is it umbrella cover? This determines how strict the deployment audit trail needs to be.
- **Tenant scope at launch.** Is Timtek targeting only DSARLD initially, or is it designed multi-tenant from day one (Sussex SAR, Eye In The Sky, etc.)? The franchise/team-of-teams pattern (Eye In The Sky) implies an extra hierarchy layer above [[Roles|Case Manager]].
- **The Missing Animal Response Network / Kat Albrecht material** — the canonical North American methodology for pet detective work — could not be fetched (site down, archive 503). Worth a manual look by Tim or a follow-up research pass when the site returns.
- **Beauty's Legacy operational detail** — the Volunteers page returned only Wix CSS. A second pass via a Wix-aware extractor or a direct conversation with Lisa Dean would fill out the small-charity workflow picture.
- **Theft-case rate.** What proportion of DSARLD cases are stolen vs lost? This determines how much UI weight to give the theft variant. The DSARLD homepage shows roughly 102 active cases at observation; their status breakdown should be easy to pull internally.

---

_End of research. See also: [[Competitive Comparison]], [[Functional Specification]], [[Roles]], [[Keepers]]._
