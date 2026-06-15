# Why the DSARLD Website Isn't Working — and What Would

_A report prepared for Drone SAR For Lost Dogs UK by Tim Long (trading as Timtek Systems), 2026-06-14._

_Tim is a DSARLD-affiliated drone pilot, a software engineer in the Maritime Search and Recue sector, and the author of an in-progress open source lost-pet SAR case manager. This document is intended as a discussion starter, not a sales pitch. The objective is to share what the evidence actually says about why digital tools succeed or fail in volunteer coordination work, and what that means for DSARLD's options going forward._

---

## The headline, up front

You haven't failed because you got the design wrong. You've hit the same wall every volunteer-coordination project hits, in every charity sector, the world over. The pattern is well-documented and the failure modes are predictable. None of this is unique to DSARLD; what's encouraging is that the patterns that *do* work are equally predictable, and they aren't out of reach.

There are three big findings in the research that matter most for the decision in front of you:

1. **The current website's "I can help / I can't help" binary is one of the single best-documented ways to make a volunteer drop out silently.** This isn't a UI quirk — it's a recognised failure mode with peer-reviewed evidence behind it. The recent poster who said "I never pick either one" is the canonical example of the documented effect, not an outlier.
2. **Going back to Facebook-only would solve your engagement problem but trade it for a different, harder, slower problem.** Facebook's strengths (reach, familiarity, frictionless posting) are real. So are its weaknesses (no audit trail, no formal closeout, no insurance record, no organisational memory beyond what individual coordinators remember). Either path has costs.
3. **A purpose-built tool can solve the engagement problem *without* trading away the audit trail and organisational memory** — but only if it's designed around how volunteers actually think and behave, not how coordinators wish they would. Most purpose-built volunteer systems fail because they're built by, and for, the coordinators.

The rest of this report unpacks each of those three findings, cites the relevant evidence, and lays out what the options actually look like.

---

## Part 1 — Why the current website is stalling

### The button-labelling trap

When a pilot or ground searcher receives an alert by email, the call-to-action button opens a web page with two choices:

- **I can help with this search**
- **Can't attend this one**

On its face this looks like a clear, simple question. It isn't, for two related reasons.

**First, the choices don't say what they actually do.** Operationally, clicking "I can help" doesn't commit the pilot to attending. It just adds them to the team for that case so they can monitor what's happening and decide later whether they can turn up. But the volunteer reading the button has no way to know that. "I can help with this search" reads — to any reasonable English speaker — as a personal commitment to attend. A cautious pilot who isn't 100% sure they can make it doesn't want to make a promise they might break, so they don't click it. The button's *label* is asking for a commitment that the *system* doesn't actually need.

**Second, the other choice is effectively a dead end.** "Can't attend this one" stops further alerts going to that volunteer for that case. In principle, the volunteer can still visit the website later and add themselves back — but that requires the pilot to remember the case exists, remember to check, log in, find it, and re-engage on their own initiative. In practice almost nobody does that. The system has quietly switched from **push** (it tells you when something needs your attention) to **pull** (you have to come and look). Push-then-pull dispatch only works for the small minority of volunteers who habitually check in. Everyone else just hears silence.

This is worth dwelling on for a moment, because it has a hidden organisational consequence. The pilots who *do* habitually check in — the ones who notice when a case is still open and re-add themselves — are probably the ones the org's leadership hears from most often. Which means the leadership's intuitive sense of "how the system is working" is shaped overwhelmingly by the people for whom it *is* working. The silent majority — the ones who clicked "Can't attend" once and then never came back — are invisible. They don't complain. They don't unsubscribe. They just stop existing in the org's day-to-day experience. The most engaged pilots are exactly the wrong sample for diagnosing engagement problems.

The combination is corrosive. A pilot who's willing-but-cautious — which describes most of them most of the time — has no honest button to click. Saying "I can help" feels like overcommitting; saying "Can't attend" cuts them off from the case. So they pick neither. They close the tab. The coordinator sees silence and assumes apathy. The pilot remembers the awkwardness and the next alert tends to get ignored too.

This is the textbook **silent drop-off** pattern. VolunteerHub's industry analysis (a US volunteer-management platform with operational data across thousands of organisations) found that confusing or mislabelled response choices are among the single largest causes of new-volunteer drop-off, and that the drop-off is invisible to the organising team because nobody who quietly disengages writes in to say so. By the time the organisation notices a participation problem, the affected cohort has already left.

A forum post from today — the pilot who said the binary choice meant he never selected either — is not an outlier. He's a volunteer who articulated what almost everyone else just acts out silently. Take him seriously; for every one person who explains it, several more just close the tab.

### What a working response model looks like

The pattern that works in comparable systems gives the volunteer a button that maps to what they actually want to say. For this case, that's probably four options:

- **I'm going** — committed, en route or imminently. Coordinator can plan around this.
- **Keep me posted** — add me to the team so I can follow this and decide later. This is the honest label for what "I can help" currently does internally, and would probably be the most-clicked option of the four.
- **Not this one** — drop me from this case's alerts, but keep asking me about future cases as normal. Lets the pilot say "wrong terrain / too far / not my modality / not free today" without resigning from the org.
- **Pause my alerts until** *[date]* — covers holiday, work trip, illness, family event. Clean way to handle absence without anyone having to explain themselves.

Each option is honest. None of them ask the volunteer to make a promise they aren't sure they can keep. The coordinator gets useful signal — "three pilots said 'keep me posted' and one said 'I'm going'" is something they can dispatch around, whereas three silences and one yes is not. Optionally, "Not this one" can offer a one-tap reason (terrain, distance, modality, capacity) so the coordinator learns whether the alert was wrongly targeted or whether the pilot was simply busy.

PulsePoint, the most successful citizen-responder system in the world (3,300+ deployments, peer-reviewed evidence of a 14-percentage-point absolute increase in bystander CPR before paramedics arrive), uses a multi-state response model. It is not coincidence that it works.

### The volunteer-management literature is brutal about this

The pattern goes by several names — "asymmetric commitment", "binary response burden", "button-meaning gap" — and shows up in academic literature on volunteer retention, in crisis-response platforms, in mutual-aid networks, in mountain-rescue systems, and in citizen-science platforms. The finding is consistent: when the choices on offer don't honestly describe what they do, or when the only options force a commitment people can't safely make, participation drops. When the decision space matches the way humans actually think about commitment, participation rises.

The Qomon volunteer-engagement survey (2023) found that **roughly 40 percent of volunteers who sign up to confusing or all-or-nothing systems never take a single action**. They aren't unmotivated. They're stuck in a UI that doesn't give them an honest answer to give.

So: the website didn't fail because of bad luck, and it didn't fail because pilots are unmotivated. It failed because the response model asks for promises the volunteer isn't sure they can keep, while hiding the option they'd actually take. People respond rationally by not answering at all.

---

## Part 2 — What going back to Facebook-only would actually mean

Facebook is genuinely tempting. It has zero install friction, it's where people already are, posting takes seconds, and the reach is real — Lost Dogs of America's Facebook network coordinates roughly 75,000 reunions across all 50 US states through groups with tens to hundreds of thousands of members each. Pet FBI in the United States explicitly relies on Facebook for distribution. So it's not a bad medium for some of the work.

But it's important to be honest about what going Facebook-only would buy and what it would cost.

### What Facebook-only would fix

- **The engagement problem disappears overnight.** No login, no binary buttons, no silent drop-off. People already use Facebook, so participation is whatever Facebook participation already is.
- **Posting is fast.** Coordinators can put a case up in 30 seconds without thinking about data fields.
- **Reach is wide.** A shareable post can travel far beyond DSARLD's volunteer pool — civilians who would never join a SAR org will still share a lost-dog post.
- **No build cost, no hosting cost, no maintenance cost.**

### What Facebook-only would quietly take away

These are the costs that don't show up in week one but compound over months and years.

- **No audit trail for insurance.** DSARLD provides pilot insurance during active searches. If a pilot has an incident — third-party property damage, a near-miss with another aircraft, anything — the insurance claim depends on being able to demonstrate the pilot was operating under DSARLD's umbrella at the time of the incident. A Facebook chat thread is not an audit trail. It's not searchable, it's not exportable, it's not signed, it's not time-stamped in a way that survives scrutiny. One contested insurance claim and this becomes a serious problem.
- **No organisational memory beyond individual coordinators.** When a coordinator burns out, takes a long break, or moves on, what they knew goes with them. Case histories, pilot reliability patterns, what worked in similar terrain, who's good with skittish dogs — none of it survives on Facebook in a useful way. Beauty's Legacy, Lost Dogs of Wisconsin, and most other successful pet-SAR orgs have a known fragility around their founders: when the founder steps back, the org wobbles or dies. Facebook-only locks that fragility in permanently.
- **No formal case closeout.** Most Facebook-coordinated cases simply scroll off the page. There's no moment of "this case ended this way." That's bad for stats (you can't tell the public your reunion rate if you don't formally close cases), bad for Gift Aid claims, bad for grant applications, and bad for the volunteers themselves — humans need closure for their own motivation. The case that ends in a rainbow-bridge outcome especially needs to be acknowledged, not just forgotten.
- **No systematic dispatch.** On Facebook, whoever happens to be online and tags themselves first becomes the responder. There's no concept of "this case is in your area, and we'd like you specifically to look at it." The result is uneven coverage — some pilots see everything, others see what their algorithm happens to surface, and the system can't tell who didn't see what.
- **No keeper continuity.** A panicked keeper at 2am needs a single, calm, structured place to give information and receive guidance. A scrolling Facebook thread with comments from dozens of well-meaning strangers is the opposite of that. The literature on the first 24 hours after a pet goes missing is unanimous: the keeper is in shock, frequently chases the dog and makes things worse, and needs clear, repeated, structured guidance ("DO NOT CHASE", "do this instead"). Facebook does not deliver that.
- **No safeguarding control.** Public Facebook posts containing a keeper's full address, phone number, and home details have been documented as creating stalking and scam risks. The Pet FBI organisation in the US has had to take down posts for exactly this reason. UK GDPR makes this DSARLD's legal problem, not Facebook's.
- **Platform-dependence.** Facebook changes its algorithms, its API, its policies, and its group rules without notice. Several adjacent organisations have lost reach overnight to algorithm changes they had no warning of. Building your whole operation on a platform you don't control is a slow-motion risk.
- **No structured handover between modalities.** When a thermal drone pilot raises a contact, they need a ground team there within roughly 20 minutes before the contact moves or cools. Facebook tagging is not a mechanism for that — it relies on someone being online, seeing the tag, and self-dispatching. Half the time the contact evaporates before anyone gets there, and the pilot resents the wasted trip. This is the single most-cited frustration of working pet-SAR drone pilots.

### The honest comparison

A reasonable summary: **Facebook gives you reach and engagement; a structured tool gives you operational discipline and continuity.** Neither is wrong. The question is what you most need.

If DSARLD's strategic future is "grow the volunteer pool and reach more keepers", Facebook helps. If DSARLD's strategic future is "operate consistently, retain pilots for years, satisfy insurers, qualify for grants, and survive the loss of any individual coordinator", Facebook quietly works against you.

In practice, almost every successful operational SAR organisation — mountain rescue, lifeboats, coastguard — uses Facebook for *outreach* and a structured system for *operations*. They are different jobs. Facebook is a good tool for one of them and a bad tool for the other.

---

## Part 3 — What a fresh, purpose-built system could actually do

This section is deliberately non-specific to anything I might or might not build. The point is to describe what's technically achievable and operationally realistic, so you have a sense of what's on the table.

### Things that solve known DSARLD pain points

- **A multi-state pilot response, not a binary one.** Fixes the silent drop-off problem from Part 1. This is a small change that has outsized effect.
- **Geographic auto-matching of pilots to cases.** A pilot defines an Area of Operation — by radius, by postcode, or by drawing a polygon — and only gets notified about cases in that AO. Eliminates the "I keep getting alerts for cases on the other side of the country" frustration that drives people to mute notifications, after which they're effectively gone. (This pattern has the strongest peer-reviewed evidence behind it of any feature in the lost-pet SAR space — PulsePoint's RCT in Stockholm showed a 14-percentage-point absolute increase in bystander CPR from text-based geographic dispatch.)
- **Per-day notification cap and quiet hours.** Volunteers can set "no more than three a day" and "nothing between 22:00 and 07:00 unless I opt in." Removes the second-biggest cause of attrition (notification fatigue).
- **A keeper-facing intake that takes the panic into account.** Single-column form, large fonts, "I can't find this right now" escape on every field. No password required (a time-bound access token sent to the keeper's phone instead). First screen after submitting is "DO NOT CHASE — here's what to do instead", not buried in a help section.
- **Structured sightings triage.** A central place where bystander sightings come in, get verified against the case timeline, get filtered for likely false positives (cats mistaken for dogs, plastic bags, well-meaning but wrong reports), and only the credible ones go out to active searchers. Cuts the noise that currently drowns useful signal.
- **Pilot-to-ground handover as a first-class workflow.** When a pilot raises a thermal contact, the nearest available ground searchers get notified instantly with a one-tap "on my way / can't make it" reply, and the pilot sees their ETA in real time. Solves the single most-cited frustration of working pet-SAR pilots.
- **A proper audit trail.** Every case has an immutable record of who was dispatched, who acknowledged, who attended, when each event happened, what was found. Searchable, exportable, signed. Defensible to insurers, to grant-makers, to ICO if ever needed.
- **Formal case outcomes.** Reunited / Recovered / Rainbow Bridge / Cold Case. Closed properly. The volunteers who worked the case know how it ended. The org has real stats to publish. The keeper gets closure.
- **Activation gap measurement.** The system records when a case was opened and when the first pilot acknowledged. Suddenly you can see — for the first time — whether your average activation gap is 15 minutes, 2 hours, or a day. You can't improve what you don't measure.

### Things to keep modest expectations about

- **A purpose-built tool will not, by itself, increase the volunteer pool.** It might increase the *engagement* of the pool you already have. Growth is a separate, mostly-marketing problem.
- **There is a transition cost.** Even the best replacement system has a period when the org is running two things in parallel and people are confused. Plan for it; it's real.
- **Software does not replace coordinators.** The most successful organisations in the research are held up by 1 to 4 people who carry the operational knowledge. The right software amplifies them. The wrong software tries to replace them, and fails. Any tool worth using is one your coordinators would describe as "saving them time", not "telling them what to do."
- **Custom development is not free.** Either it's built by a volunteer (who can disappear), by a company (which costs money), or by a member of the org (who absorbs the maintenance burden forever). All three are viable paths but none are free.

### The third option that often gets missed

It is not a binary choice between "the current website" and "Facebook-only." There's at least a third option, which is **Facebook for outreach, structured tool for operations**, run in parallel deliberately. Lost Dogs Georgia coordinates across eight separate consumer SaaS products. It works, sort of, but it's brittle. A purpose-built tool can replace most of the operations side while still publishing the public-facing flyer to Facebook for reach. That's probably the realistic shape of a working future, not "all of one or all of the other."

---

## What the research base actually says

This isn't speculation. The findings above are drawn from a structured review of the published evidence on volunteer engagement, citizen-responder systems, and pet-SAR operational practice. The key sources, in plain English:

- **PulsePoint deployment data and Stockholm RCT.** A peer-reviewed randomised controlled trial showed that geographically targeted text dispatch of citizen responders produced a 14-percentage-point absolute increase in bystander CPR before paramedics arrived. PulsePoint is deployed in 3,300+ US communities. This is the closest direct evidence we have for what geographic auto-matching can achieve in a comparable domain.
- **VolunteerHub volunteer-management analysis.** Industry data across thousands of nonprofits showing that ~40% of new volunteer signups never take a first action, and that confusing or binary UI is the leading documented cause.
- **Qomon 2023 volunteer engagement survey.** Confirms the silent drop-off pattern, with specific data on binary commitment framing as a top driver of disengagement.
- **Lost Dogs of America operational data.** ~75,000 documented reunions across all 50 US states, operated via Facebook groups. Validates Facebook's reach for outreach; coordinators within the network are open about its limits for ops.
- **Kat Albrecht and the Missing Animal Response Network.** The closest thing to an academic basis for pet-recovery practice. Albrecht's behavioural taxonomy of lost dogs (skittish / social / aloof / xenophobic / hurt) is the single most evidence-backed pet-specific signal in the entire literature, and it determines what search pattern works.
- **Stack Overflow's documented decline.** The canonical case against publicly-visible reputation and gamification systems for volunteer communities. Visible scores became the goal; high-rep users became gatekeepers; intrinsic motivation collapsed; traffic fell. Several volunteer platforms have repeated the same mistake.
- **ICO and NCVO guidance on UK GDPR for charities.** UK GDPR applies to every charity that processes personal data, with no exemption for size or volunteer status. The ICO has fined charities specifically for untrained volunteers mishandling personal data. Lost-pet SAR data (keeper PII, home address, contact details) is exactly what the regime is built to regulate.
- **Working pet-SAR drone pilot forum discussions and commercial pilot workflows.** Consistent themes: late activation, no last-known-position briefing, keeper-on-scene complications, thermal-as-magic-bullet myth, and scent-vs-thermal handover gap as the single most cited frustration.

A fuller writeup with sources and links is available on request.

---

## So what should DSARLD do?

That's genuinely DSARLD's call, not the report's. But the three honest options look like this:

**Option A — Stay on the current website.** The engagement problem is now well-understood; a focused fix to the response model alone (binary → multi-state) would probably recover a noticeable fraction of the silent drop-out. Cheapest path. But it doesn't address the wider operational gaps.

**Option B — Go Facebook-only.** Engagement recovers fast and the org regains its momentum. The longer-term costs (no audit trail, no closeout, no organisational memory, platform dependence) are real and will compound. Workable if the strategic priority is reach and the operational gaps are accepted.

**Option C — Move to a purpose-built tool, using Facebook for outreach only.** Highest setup cost; addresses both the engagement and the operational issues. The right answer if DSARLD's medium-term direction is to operate as a more formal SAR organisation — surviving founder turnover, satisfying insurers, qualifying for grants, and producing publishable outcome statistics.

There's no objectively right answer; it depends on where DSARLD wants to be in five years. What I'd hope this report helps with is making the trade-offs visible so the decision is informed, not reactive.

Whatever the choice is, please don't read the current website's stall as a failure of the people who built it or of the pilots who didn't engage. It's a well-documented industry pattern with a known cause. That makes it fixable.

---

## Who I am, and what I'm offering (and what I'm not)

I'm Tim Long. I fly an 8-inch FPV drone, I'm a DSARLD-affiliated pilot, and I've spent 25 years as a software developer (currently at Ocean Signal Limited in Margate, a maritime safety equipment manufacturer). I'm building a multi-tenant lost-pet SAR case manager in my own time. It's in active development.

### What I'm not

I'm not selling anything. There is no product, no contract, no subscription, no "trial period that converts", no upsell, no commercial relationship of any kind to commit to. I'm a volunteer pilot offering volunteer software skills to a volunteer organisation.

### What I'm offering

If DSARLD wanted to try it, the offer would look like this:

- **The software itself, free.** Built in my own time, used at DSARLD's discretion.
- **My time to set it up, free.** Installation, configuration, data migration where it makes sense, ongoing tweaks.
- **The source code, open source and MIT-licensed from day one.** That's the most permissive standard open-source licence: it means anyone can read, modify, redistribute, and use the code for any purpose, without paying anyone. DSARLD would not be locked in to me, to my company, or to any particular hosting arrangement.
- **The only running cost would be hosting** — a small server (a "VPS", which stands for virtual private server) at somewhere in the region of £10–£30 per month depending on usage. DSARLD would own that hosting account directly; I would not be in the middle of it.
- **A trial period with no commitment in either direction.** Run it in parallel with whatever else DSARLD is using, for as long as it takes to decide whether it's useful. If it isn't, walk away with no consequences and the data you put in handed back to you in a standard format.

### Why I'm doing this

I have my own agenda, and I want to be transparent about it. I'm a drone pilot, and the public perception of drones has become quite negative over the last few years — partly because of legitimate concerns about misuse, partly because the visible-uses-of-drones tend to be the dramatic ones (military, surveillance, near-misses with aircraft). I'd like to see more visibility for the genuinely positive uses, and pet SAR is one of the clearest examples of drones doing humanitarian work. Helping a SAR org operate more effectively serves that goal. It also gives me a real-world, real-stakes project to develop my own engineering skill on. Both of those motivations sit alongside (and behind) any direct benefit to DSARLD.

In the longer term, I may offer the same software as a service to other lost-pet SAR organisations — different countries, different regions, different species. Any such future use would be on the same terms (open source, MIT-licensed, organisations own their own data) and would never restrict or affect DSARLD's use. DSARLD would not be funding a future commercial product; DSARLD would be benefiting from the same software that would later help other groups.

### What happens if I get hit by a bus

This is a fair question and I want to address it head-on, because it's the right one to ask of any small-team or single-developer project.

The honest answer in three parts:

1. **The source code is and always will be MIT-licensed and openly published.** That means if I disappear tomorrow, the code does not. Anyone — another developer, another organisation, a hired contractor — can pick it up, run it, modify it, or maintain it. DSARLD is not depending on my personal availability for the software's continued existence.
2. **DSARLD's data would be DSARLD's data, in a standard format, on DSARLD's hosting account.** Not locked inside a service I control. Not encrypted with a key only I hold. If I vanish, DSARLD still has its own database, exportable to standard formats (CSV, JSON), readable by any competent developer.
3. **The software is built on standard, mainstream technologies** — the same ones used by tens of thousands of working software developers worldwide. There is nothing exotic, nothing proprietary, nothing that would require finding a specialist to maintain. Any competent .NET developer could pick it up.

In short: the "hit by a bus" scenario would be inconvenient (DSARLD would need to find someone else to do further development), but not catastrophic (the software would keep running, the data would still belong to DSARLD, and the source code would be available to anyone who wanted to continue the work).

This is genuinely better than the position DSARLD is in with the current website, where a custom-built system on a non-open codebase has all the same single-point-of-failure risks without the protection of open-source licensing.

### What I'm asking

I'm not asking DSARLD to commit to anything. I am asking — if there's appetite — for a conversation, because:

- Several of the design decisions in front of me would be better informed by DSARLD's real operational data than by my best guesses, and
- If a future version of the tool turned out to be useful to DSARLD, that would be a good outcome for both sides, but only if the design were grounded in DSARLD's actual workflow.

If you'd like to talk, I'm happy to do that on whatever terms suit you — informal, formal, written, in person, group session, one-to-one. If now isn't the right time, that's fine too. If this report is useful as a discussion document inside DSARLD even without any further involvement from me, that's also fine.

Either way, thank you for the work you do. The reason there's a real gap in this sector worth filling is that the work you've already done has set the bar high enough to be worth filling it well.

— Tim Long, trading as Timtek Systems, 14 June 2026.

---

_Source materials and a more detailed evidence pack are available on request. See also the supporting documents [[Operational Models]], [[Workflow and UX Evidence]], [[Sanity Check Report]] in the same folder._
