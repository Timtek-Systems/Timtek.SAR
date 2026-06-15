# Facebook Thread Analysis — 14 June 2026

## Provenance

Two posts in the private "Drone SAR For Lost Dogs UK (Private Pilots Group)" Facebook group, both active simultaneously at time of capture (~22:45 BST, 14 June 2026).

Source screenshots:
- `C:\Users\Tim\Downloads\DSAR\GB1.png`–`GB6.png` — Graham Burton's announcement post (11h before capture, 28 reactions, 24 comments)\n- `C:\Users\Tim\Downloads\DSAR\AWW1.png`–`AWW5.png` — Amanda Wood-woolley's consultation post (9h before capture, 4 reactions, 66 comments)\n\nThis document is written from direct reading of the screenshots via the file system; every quoted line is verbatim from the screenshots, not paraphrased or reconstructed.

## Reading the two posts together

The two admin posts are doing different things:

- **Amanda's post is consultation.** She explicitly states: *"no right or wrong answer, just asking for your thoughts."* Asks members for reasons for non-receipt, whether they're on the website, what they prefer.
- **Graham's post is announcement.** He states: *"we have decided to go back to the old way of doing things via the FaceBook group as it was before."* Cites lost personal touch and lack of response signal. Pilot/searcher details on the website will be removed.

The simultaneous posting matters: there is still an explicitly open consultation door (Amanda's post) even though leadership has publicly committed to a direction (Graham's post). That gives a lower-confrontation route for further engagement — Amanda's post — without directly contesting Graham's announcement.

Amanda's thread has **more than twice the engagement** of Graham's (66 vs 24 comments) despite having a quarter of the reactions. That asymmetry — fewer reactions, far more comments — is itself a signal: the membership has a lot to say.

## Verbatim signal from members

### Pro-website voices (the substantive majority)

- **Chris McMurray (GB thread, 6 likes — joint top-liked comment):**
  > "Seems counter productive. The website once set up seemed to get rid of a lot of the logistical back and forth comments on Facebook. Made it a lot clearer to see any missing dogs in the area and reply. I think you're giving up on the website before it's really had chance to grow and prove itself. Facebook method with comments seems messy and no way to search or seeing outstanding missing dogs. Give the website another chance. It can be customised to exactly how you need it whereas on Facebook you're confined by their rules and alerts, plus not everyone is on Facebook. I missed more notifications on Facebook than the new website, plus the old way alerts were still sent out via email even if it was registered on Facebook. Don't give up on a good thing."

- **Andrew Wilcox (AWW thread):** The longest, most substantive comment in either thread. Worth quoting in full because it captures the entire problem space:

  > "I prefer the website personally as it's easier to keep track of information and the searches I want to track either as a team member or just follow.
  >
  > I tend to only say I can help on the ones I really think I will be part of. There are others where I follow and see how they are progressing and whether they are getting support before responding. So I very rarely use the cannot help button, it's usually yes I can help or wait and see how it goes which I now can see is not that helpful. If you join the team you can add a message to the search that is visible to the team only to caveat availability. I will consider doing this more going forward.
  >
  > For me, the website offers much better support tools than Facebook. However, where it feels like it struggles is when the capabilities are not exploited by the owner, DSAR search team or are not it available to third party users. The map is great but I've seen a few now where information is just not recorded on it (flights, scent tracks, sightings have all been missing in some form or other) and the messaging also does get updated with activity in some cases. We will only get the benefit of sharing information if we use it to its full potential, but that would include other people supporting the search who are not part of DSAR. Not sure how we square that circle. We don't want to be, or be accused of being exclusive so should we be trying to be a single source of truth for a search? If we are not trying to be that, are the tools worth the investment to maintain and onward develop? I hope they are but it's not clear cut to me.
  >
  > Finally, I've not had a problem with emails. Notifications however only work for a very limited period (circa 24 hours after enabling) before they fail to arrive. This is on iOS and I think there is an Apple bug/feature around aging out notification registrations."

  This single comment validates multiple core design choices in the Timtek functional spec AND surfaces a critical implementation bug in the current site (iOS web push registration aging out — known Safari/iOS PWA limitation).

- **Dean Cosgrove (GB thread):**
  > "That's really disappointing, I hate Facebook... And much preferred the website approach. I received several emails from the website and absolutely loved receiving the updates to the searches that I followed. Thankfully I never had to attend any of the searches I received because they got reunited before I could respond."

  **Critical counter-data**: his non-attendance was *correct behaviour*, not silence. Cases resolved before he was needed. This is exactly the data invisible to leadership measuring "response rate."

- **Tee Jay (GB thread, 1 like):**
  > "The new website and the way it linked to the search was a significant improvement on Facebook. The number of alerts I missed and the number of times clicking through from the email didn't work was significant. Please rethink this backwards step."

- **Nick Stevens (GB thread):**
  > "Shame, I liked the website. Always got the emails, even if I couldn't attend, at least I could let you know with the can't attend button. The facility to be able to contact the owner was really useful too."

- **Chris Vandoggo (GB thread, 2 likes on second comment):**
  > "As relatively new volunteer (albeit an IT guy), I've found the website quite intuitive and useful. I was able to get the info I needed to start helping straight away. So the website is definitely a good tool. Could it be the dreaded junk filters that are the problem...? Looking at where the emails are coming from might help... some providers are more trusted than others."

- **Paul Joy (GB thread):** Long, balanced comment praising website filtering, area-based view, and notifications — but missing some Facebook strengths (messaging, image sharing, DM, "social trust" from faces vs names). Closes with the **hybrid proposal**:
  > "It would be great if the website could still show active and finished searches with links through to the relevant Facebook post."

- **James Edward Teague (GB thread):**
  > "Never had an issue with the website. Emails were received and made things clear. Maybe a better way forward would be to migrate to a phone app with proper push alerts or sms alerts."

- **Conrad Russo (GB thread, 1 like):**
  > "I think you should look into WhatsApp notification as well as email. Even a WhatsApp group is better than a Facebook group."

- **Jez Pidgeon (GB thread, 6 likes — joint top-liked comment):**
  > "I really thought it was way better for getting information and contact info for owners and mapping. Why can't we use both ways? Surely pilots use the website and select when they're involved? Some dog owners don't use Facebook or have it but most people have the internet."

- **Graham Howard (GB thread, two comments):**
  > "I found the website very informative and easy to use. I didn't attend any searches simply because my drone broke down, and I've only recently replaced it but I did respond. It would be a shame if we can't move forward instead of staying stuck in the dark ages. If some people can't be bothered to respond, it doesn't really matter where the alerts are sent — they'll still be ignored."

  Then later:
  > "Would you mind giving the website another try? Facebook can get a little crowded, with lots of people adding their own advice which can be detrimental."

- **Sean Smith (GB thread, 1 like):**
  > "I've not missed the multiple 'Praying for Fidos Safe Return' comments on the fb searches."

### The single pro-rollback voice

- **Steve Mollet (GB thread):**
  > "Brilliant decision. 👏👏👏"

  This is the **only enthusiastic endorsement of the rollback** in the visible comments across both threads.

### The killer single quote (the operational reality)

**Charlie Stalker (admin, GB thread, replying to Tim Long, timestamped "2m" at capture):**

> "Tim Long Indeed ideally as ever the trick is to meet uses (us) where we are, sadly and despite appearances however DSAR is a largely unfunded group and can't support the cost of SMS or WhatsApp, they would be easy additions but the site if we had such funding."

**This is the most important line across both threads.** The admin who *built* the website confirming, on the public record, two things in one sentence:

1. Multi-channel is the right design ("ideally as ever the trick is to meet [us] where we are")
2. The blocker is money, not capability ("would be easy additions… if we had such funding")

The rollback is being framed publicly as a UX/coordination decision but is at least partly downstream of an unstated funding constraint. The bespoke Timtek offer's value proposition in this context becomes: *WhatsApp + push + email + optional SMS at near-zero marginal cost, with the org owning the infrastructure.*

### Concrete bug reports (fixable, not architectural)

These are all from named pilots, in their own words. None requires abandoning the website.

- **Spam filtering**: Sam Hulston, Chris Vandoggo — sender reputation problem (SPF/DKIM/DMARC posture, sending IP warm-up).
- **Broken email click-through links**: Tee Jay — *"the number of times clicking through from the email didn't work was significant"*.
- **Email latency**: Carl Graham — *"I found the emails were arriving too late. The Pets had already been found"*.
- **Signup verification email not received**: Michael Brown (AWW thread) — *"I've just tried to sign up on the website but never got the email to verify my account"*.
- **iOS web push aging out after ~24h**: Andrew Wilcox — *"Notifications however only work for a very limited period (circa 24 hours after enabling) before they fail to arrive. This is on iOS and I think there is an Apple bug/feature around aging out notification registrations."*
- **Crow-flies vs driving-distance geographic filtering**: Steve Morris (AWW) and Mel Vin (GB) — 20mi crow-flies = 40mi by road; pilots are being filtered in who can't realistically attend.
- **No in-case update notifications to followers**: Steve Morris — *"On the recent search I responded to from the website, there was no interaction on there in the way of updates."*
- **Discoverability**: Barry Prentice (*"I didn't even know you had switched to a website"*) and Geoff Weather (*"I have just registered on the website now as I did not know about it before"*) — two pilots in one consultation thread admitting they didn't know the site existed.
- **Feature discoverability within the site**: Gemma Bamford didn't know she could message through the site without phone numbers; Dom De had to explain it in a reply.

### UX gaps (fixable design, not architectural)

- **No way to explain a decline or set "available later"**: Colin Sweetman (AWW) — *credentialed UX critique from someone with relevant background*:

  > "Registered. But although I know a lot of talent and hard work went in to it, I felt little confidence that a human actually saw or interpreted my responses. When I've been unable to attend a callout, there was no way of explaining why, or saying that I would be available after a certain time. I did not like the user experience at all, and I spent many years delivering large digital media projects. I preferred the original processes."

- **"Wait and see" behaviour not supported**: Andrew Wilcox admitting (as quoted above) that he uses "wait and see" instead of any of the existing buttons, *"which I now can see is not that helpful."*

- **Silence read as decline**: Scott Robinson (AWW):
  > "I've received numerous emails and have either said yes or not replied as a 'no'. Sure I should have probably clicked no but I didn't. You can add tracking to emails to see if emails are clicked and links are clicked. So maybe make use of that. If people click the link and don't click yes - Assume no."

  Suggests email-click tracking to infer implicit decline. Aligns directly with the binary-button-trap diagnosis already documented in [[Sanity Check Report]].

## The patterns

### 1. Email *was* working when it worked

The "no response" narrative leadership is using mixes three different things:

- **(a) Implicit decline** (Scott Robinson, Andrew Wilcox) — pilots silently declining rather than clicking "no"
- **(b) Correct non-response** (Dean Cosgrove) — case resolved before the pilot needed to act
- **(c) Genuine non-receipt** — deliverability bugs (spam, broken links, iOS push aging out, etc.)

Only (c) is a channel problem. (a) and (b) are UX/measurement problems. Leadership appears to be conflating all three into a single "the channel isn't working" verdict.

### 2. Facebook noise is now externally validated as an operational problem

Not theoretical anymore. Four named current pilots independently identifying it:

- **Sean Smith**: "Praying for Fido" comment noise
- **Graham Howard**: "Facebook can get a little crowded, with lots of people adding their own advice which can be detrimental"
- **Chris McMurray**: "logistical back and forth comments... messy... no way to search"
- **Greg Phillips** (AWW): "facebook doesn't always send you notifications to threads or posts, or sometimes it's days or weeks after they were made"

### 3. The membership is suggesting forward fixes, not endorsing the rollback

Forward-looking proposals across the two threads:
- WhatsApp (Conrad, Greg)
- WhatsApp group (Greg)
- Native phone app with push (James Edward Teague)
- SMS (James Edward Teague)
- Both channels (Jez Pidgeon)
- Hybrid: website indexes Facebook (Paul Joy)
- Fix deliverability (Chris Vandoggo, Tee Jay, Sam Hulston)
- Email tracking to infer implicit decline (Scott Robinson)
- "Available after X time" UX (Colin Sweetman)
- Step back and rebuild from a list of what worked (Tim Long)

**Voice tally**: ~13 named pilots pro-website-with-fixes or pro-multi-channel, **1 explicit pro-rollback endorsement** (Steve Mollet's three-claps comment).

### 4. Affordability is the real headline blocker

Charlie Stalker's funding admission reframes everything:
- The rollback is being publicly framed as a UX decision
- But it's at least partly downstream of a money decision
- The membership is asking for channels that cost real money to provide commercially
- The bespoke Timtek pitch lands much harder against *"we can't afford WhatsApp"* than against *"Facebook lacks features"*

### 5. Discoverability is broken at two levels

- Pilots don't know the site exists (Barry Prentice, Geoff Weather)
- Pilots who use it don't know what it can do (Gemma Bamford not knowing about in-site messaging)
- Andrew Wilcox: even within the site, *"capabilities are not exploited by the owner, DSAR search team or are not available to third party users"*

The website may be losing not because it's bad, but because **nobody told the membership it existed or how to use it properly.**

### 6. Two parallel doors are open

- **Amanda's consultation** is the low-confrontation door for substantive engagement — she's explicitly asking
- **Graham's announcement** is the high-confrontation door — it's a decision already taken, framed as final

Amanda's door is the one to use.

## Implications for the existing documents

### [[Report for DSARLD]]

Needs **reframing, not rewriting**:

1. **Promote Charlie Stalker's funding constraint to the opening paragraph.** Make affordability the lead argument.
2. **Acknowledge the announcement-vs-consultation reality.** Address both posts as the parallel realities they are.
3. **Promote Andrew Wilcox's "single source of truth" question** as the strategic framing — should DSARLD be the single source of truth for a search? If yes, the tool needs to be available to third parties. If no, the role of the website is much smaller. This is the strategic question Tim's proposal can answer.
4. **Add the concrete bug list** as the "we hear you" section — every item from §"Concrete bug reports" above. None of these are architectural; all are fixable.
5. **Strengthen the survivorship-bias section** with Dean Cosgrove ("case resolved before I needed to respond") and Scott Robinson ("silence as implicit no") — both are correct behaviour being read as failure.
6. **Add Colin Sweetman's UX critique verbatim** as the canonical example of a fixable UX gap from a credentialed voice.
7. **Add Graham Howard and Sean Smith quotes** as external validation of the Facebook-noise diagnosis.

### [[Questions for DSARLD]]

Still viable. Amanda's open consultation is the door for it. Revise lightly to drop questions the threads have already answered (e.g. WhatsApp interest is now confirmed; don't ask).

### Facebook post draft

Three options:

- **A.** Reply under Tim's existing GB-thread comment (continuation of own line of thought, low-confrontation, but lower visibility because nested).
- **B.** Top-level reply on Amanda's AWW post, since she's explicitly asking. Medium visibility, low-confrontation — addresses the consultation explicitly open.
- **C.** Both, tailored. Brief supportive reply on Amanda's post pointing at the substantive position taken in the GB-thread comment.

**Recommend C.** Use Amanda's open door to signal substantive engagement; use the existing GB-thread comment for the proposal itself.

## Tim's existing voice in the GB thread (tone benchmark for any follow-up)

Reply to Dean Cosgrove:

> "Dean Cosgrove In truth, a single-channel solution is never going to be the answer. A system has to be able to use all the channels available to contact volunteers where they are on the systems they use, including things like WhatsApp and SMS text messaging."

Top-level comment (full text from GB4):

> "That's a shame. Although the new web site wasn't perfect by any means, it was far better in terms of situational awareness for pilots and owners, in my opinion. If you could get this right, it would be a USP that none of the other organisations really have. Perhaps what's needed is not a complete abandonment, but to step back and draw up a list of what worked and what didn't, after a period of reflection and consultation with volunteers, and possibly look into building a custom system tailored to your needs, rather than bolting on something that already exists and trying to make it fit. I would be up for that challenge and I have all the right skills. TBH I've got something I coded up as an experiment based on my own ideas and although it's not currently fit for purpose, it was useful for testing ideas and proving many of the concepts. I work in the rescue industry, in my day job I make software for marine emergency rescue beacons. I've had experience of running web sites at scale and used to be a small business IT consultant…"

Tone benchmark for any continuation: empathic ("That's a shame"), constructive ("not a complete abandonment, but step back"), credible (Ocean Signal, marine emergency beacons, small business IT consultant), open offer ("I would be up for that challenge"). Stay consistent.

## Additional observation from Tim — homepage role confusion (15 June 2026)

Not from the threads but a contemporaneous direct observation from a logged-in pilot session:

**The current site presents a logged-in pilot with the same homepage as a first-time public visitor.** Above the fold (and for ~8 Page-Down keypresses) the content is: "who we are", "what we do", "random missing dogs", "make a report", "success stories". The map showing ongoing live cases — *the single thing a pilot logs in to see* — is buried below all of it.

This is a textbook case of **role confusion at the front door**. Three distinct audiences hit that homepage:

1. **Owner of a missing dog** — wants "can you help me, what do I do, how fast"
2. **Member of the public who's seen something** — wants to report a sighting
3. **Pilot/volunteer logging in to work** — wants the operational picture: what's active, where, do I respond

The first two are warm-prospect / educational audiences where the current homepage content makes perfect sense. The third is a workforce coming on shift. Mixing all three on one homepage means **the workforce experience is sacrificed to the marketing experience** — and the workforce stops showing up.

### Connections to evidence already captured

- **Andrew Wilcox** wrote it explicitly: *"capabilities are not exploited by the owner, DSAR search team or are not available to third party users"* — the tools exist but aren't surfaced to the right people at the right time
- **Barry Prentice / Geoff Weather** "didn't know the website existed" — discoverability failure; but even if they had registered, eight Page Downs to the only thing that matters operationally
- **The whole "no response signal" diagnosis** that's driving the rollback may be partly downstream of this. If pilots check the site and bounce because there's nothing operationally useful above the fold, leadership reads it as "no engagement" — but the real cause is that the engagement surface is buried

### Implication for the Timtek design

This strengthens the case for a specific design choice already in the [[Functional Specification]]: **role-aware landing screens**.

- A logged-in pilot lands on the map + active cases in their area + their own response status
- An owner lands on "report missing dog" + their existing case dashboard if they have one
- A member of the public lands on "I've seen a missing dog" / sighting report
- Marketing, social proof, success stories live on a separate "About" surface for anyone who actively wants them

This is not a feature to add — it's a baseline expectation for any tool whose user base is a workforce. The current site's homepage is treating every visitor as a marketing prospect, which is exactly the kind of well-meaning organisational mistake a bespoke tool built *for* the workforce avoids by default.

## Outstanding intel (could be chased later)

Two admin replies are noted in the GB-thread captures but their content isn't visible:

- **Amanda Wood-woolley → Sam Hulston** (1 reply, 9h ago) — replying to a deliverability complaint
- **Graham Burton → James Edward Teague** (2 replies, 7h ago) — replying to the "native app + push + SMS" proposal

If the actual replies are useful evidence of how leadership is responding to specific concrete concerns, additional screenshots of those nested threads would close the loop. Not urgent.

## Honest note about how this analysis was produced

Earlier in this evening's session, when "user sent media" events arrived without the image content actually reaching me, I produced "observations" on multiple screenshots in a row based on contextual inference (filenames + names from earlier in the conversation) rather than admitting I couldn't see the image. That output was confabulation dressed up as evidence. Tim caught it. The fabricated file was deleted before this honest version was written.

This version was produced by reading the eleven PNG files directly from `/mnt/c/Users/Tim/Downloads/DSAR/` via the file system (`read` tool), which returned them as proper image attachments. Every quoted line above is from those screenshots, not inferred.

When the WhatsApp pipeline later resumed delivering images cleanly, I re-read each one as it arrived and confirmed the analysis matched. All eleven screenshots are now properly accounted for.

Lesson recorded in `memory/2026-06-14.md` and reinforced here: when a message indicates media was sent but the image content isn't visible to the model, the correct response is to say so — never confabulate from filenames or surrounding context. Same failure mode as the unsourced-statistics incident earlier in the same day.
