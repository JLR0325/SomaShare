# SomaShare - Deployment Checklist

## Pre-Deployment Verification

### ✅ Code Integration

- [ ] **ChatController.cs** copied to `Controllers/` directory
- [ ] **ProfileController.cs** copied to `Controllers/` directory
- [ ] **LanguageController.cs** copied to `Controllers/` directory
- [ ] **Chat views** copied to `Views/Chat/` directory
- [ ] **Profile views** copied to `Views/Profile/` directory
- [ ] **Shared partials** (_Navbar, _LanguageToggle, _UserCard) in `Views/Shared/`
- [ ] **Enhanced views** reviewed and ready for integration

### ✅ Layout Updates

- [ ] `_Layout.cshtml` updated with `@await Html.PartialAsync("_Navbar")`
- [ ] Bootstrap 5+ link included in `_Layout.cshtml`
- [ ] Font Awesome 6+ link included in `_Layout.cshtml`
- [ ] `_ViewImports.cshtml` has necessary `@using` statements

### ✅ Database

- [ ] Latest migration applied: `Update-Database`
- [ ] `ChatMessage` table exists: `SELECT COUNT(*) FROM ChatMessages`
- [ ] `ApplicationUser` table has `ProfileImageUrl` column
- [ ] `ApplicationUser` table has `Rating` column
- [ ] Foreign keys established correctly

### ✅ Configuration

- [ ] `Program.cs` has DbContext configured
- [ ] `Program.cs` has Identity configured
- [ ] `Program.cs` has MVC services configured
- [ ] Connection string valid in `appsettings.json`
- [ ] HTTPS enabled for development/production

---

## Feature Testing

### 🔐 Chat System Tests

- [ ] Visit `/Chat/Index` (requires authentication)
- [ ] No conversations initially (empty state works)
- [ ] Can navigate to `/Chat/Conversation/{validUserId}`
- [ ] Chat view displays other user's profile info
- [ ] Message input form present and functional
- [ ] Send button works
- [ ] Message saves to database
- [ ] Auto-refresh displays new messages
- [ ] User can see conversation history
- [ ] Other user receives message

### 👤 Profile System Tests

- [ ] Visit `/Profile/View/{validUserId}` (no auth required)
- [ ] Profile displays correct user information
- [ ] Rating displayed with stars
- [ ] Profile picture shows (or placeholder if missing)
- [ ] Statistics display correctly
- [ ] Authenticated users see "Send Message" button
- [ ] "Send Message" links to `/Chat/Conversation/{userId}`
- [ ] "View Profile" button works
- [ ] Own profile has "Edit Profile" button

### 🌍 Language Toggle Tests

- [ ] Navbar displays "Language" dropdown
- [ ] 3 language options visible (English, Français, Zulu)
- [ ] Clicking language changes cookie
- [ ] Page redirects to same URL after language change
- [ ] All 3 languages work without errors
- [ ] Language preference persists across sessions
- [ ] Flag emojis display correctly

### 📚 Integration in Listings

#### Textbook Index
- [ ] Each textbook card shows seller info
- [ ] Seller profile picture displays
- [ ] Seller name is clickable link to profile
- [ ] Seller rating displayed
- [ ] Institution visible
- [ ] "Contact Seller" button works
- [ ] Search/filter functions work
- [ ] Responsive on mobile

#### Textbook Details
- [ ] Large seller profile card present
- [ ] Seller information complete
- [ ] "View Full Profile" button works
- [ ] "Send Message" button works
- [ ] Offer section shows offeror info
- [ ] All links navigate correctly

#### Wanted Ads
- [ ] Requester info displayed on each card
- [ ] Profile pictures visible
- [ ] Names are clickable
- [ ] Ratings displayed
- [ ] "Contact Requester" button works
- [ ] Responsive design

#### Offers
- [ ] Offeror information visible
- [ ] Price comparison clear
- [ ] Status badges display correctly
- [ ] Seller profile info in details
- [ ] Accept/Decline buttons work (for sellers)

### 🧭 Navigation Tests

- [ ] Navbar displays correctly
- [ ] All links navigate to correct pages
- [ ] Dropdown menus work
- [ ] Mobile hamburger menu works
- [ ] User menu displays when authenticated
- [ ] Login/Register links show when not authenticated
- [ ] Logout button works
- [ ] Logo links to home

---

## Performance & Optimization

- [ ] Chat messages load within 2 seconds
- [ ] Profile pages load within 1 second
- [ ] Language toggle instant (cookie-based)
- [ ] Images optimized for web
- [ ] No console errors on any page
- [ ] No SQL N+1 queries (check profiler)
- [ ] Database queries include necessary `.Include()`
- [ ] Pagination works for large listings

---

## Security Verification

- [ ] HTTPS enabled in production
- [ ] Anti-CSRF tokens on all forms
- [ ] User authentication required for chat
- [ ] Authorization checks on profile edits
- [ ] SQL injection prevention (EF Core parameterized queries)
- [ ] XSS protection (Razor template encoding)
- [ ] CORS headers configured if needed
- [ ] Sensitive data not logged

---

## Browser Compatibility

- [ ] Chrome/Edge (latest)
- [ ] Firefox (latest)
- [ ] Safari (latest)
- [ ] Mobile Safari (iOS)
- [ ] Chrome Mobile (Android)

---

## Mobile Responsiveness

- [ ] Text readable on small screens
- [ ] Buttons touch-friendly (>44px)
- [ ] Images scale correctly
- [ ] Forms work on mobile
- [ ] Navbar hamburger menu functional
- [ ] Dropdowns work on touch
- [ ] No horizontal scrolling
- [ ] Viewport meta tag present

---

## Error Handling

- [ ] Graceful error pages (404, 500)
- [ ] Invalid user IDs show 404
- [ ] Authentication failures handled
- [ ] Database errors logged
- [ ] Validation messages display clearly
- [ ] Toast/alert notifications work

---

## Deployment Steps

1. **Backup Database**
   ```bash
   # Backup production database before deploying
   ```

2. **Build Application**
   ```bash
   dotnet clean
   dotnet build -c Release
   ```

3. **Run Tests**
   ```bash
   dotnet test
   ```

4. **Publish**
   ```bash
   dotnet publish -c Release -o ./publish
   ```

5. **Apply Migrations**
   ```bash
   dotnet ef database update --configuration Release
   ```

6. **Start Application**
   ```bash
   dotnet SomaShare.dll
   ```

---

## Post-Deployment Verification

- [ ] Application starts without errors
- [ ] Database connected successfully
- [ ] All pages load correctly
- [ ] Chat system functional
- [ ] User profiles visible
- [ ] Language toggle works
- [ ] Listings show user information
- [ ] Navigation working
- [ ] Login/Register functional
- [ ] Monitor logs for errors

---

## Rollback Plan

If issues occur:

1. **Immediate Actions:**
   - Stop application
   - Check error logs
   - Identify root cause

2. **Database Rollback:**
   ```bash
   dotnet ef database update {previousMigrationName}
   ```

3. **Code Rollback:**
   ```bash
   git revert {commitHash}
   dotnet publish -c Release
   ```

4. **Restore from Backup:**
   - Use pre-deployment database backup
   - Restart application

---

## Monitoring & Maintenance

### Daily Tasks
- [ ] Check error logs
- [ ] Verify database connectivity
- [ ] Monitor application performance
- [ ] Check user feedback

### Weekly Tasks
- [ ] Review server logs
- [ ] Update security patches
- [ ] Check for deprecation warnings
- [ ] Backup database

### Monthly Tasks
- [ ] Performance analysis
- [ ] Database maintenance
- [ ] Security audit
- [ ] Feature review

---

## Support & Documentation

- [ ] Share IMPLEMENTATION_GUIDE.md with team
- [ ] Share QUICK_START.md with developers
- [ ] Document any customizations made
- [ ] Create runbooks for common issues
- [ ] Document deployment procedure
- [ ] Share access credentials securely

---

## Stakeholder Sign-Off

### Development Team
- [ ] Code reviewed and approved
- [ ] Tests passed
- [ ] Documentation complete
- [ ] Ready for deployment

### QA Team
- [ ] All features tested
- [ ] No critical bugs
- [ ] Performance acceptable
- [ ] Security verified

### Product Team
- [ ] Feature complete
- [ ] User requirements met
- [ ] Ready for users
- [ ] Deployment approved

### Operations Team
- [ ] Infrastructure ready
- [ ] Monitoring configured
- [ ] Backup procedure tested
- [ ] Support plan ready

---

## Go/No-Go Decision

**Date:** _______________

**Decision:** ☐ Go | ☐ No-Go | ☐ Conditional

**Reasons:**
_________________________________________________________________

**Sign-Off:**

| Role | Name | Signature | Date |
|------|------|-----------|------|
| Dev Lead | _____________ | _____________ | ______ |
| QA Lead | _____________ | _____________ | ______ |
| PM | _____________ | _____________ | ______ |
| Ops Lead | _____________ | _____________ | ______ |

---

## Post-Deployment Monitoring

### First 24 Hours
- Monitor error rates
- Check database performance
- Verify all features working
- Gather user feedback
- Check application logs hourly

### First Week
- Daily log reviews
- Performance metrics analysis
- User feedback response
- Bug tracking
- Optimization opportunities

### Ongoing
- Weekly performance reports
- Monthly security reviews
- Quarterly feature enhancements
- User satisfaction tracking

---

## Contact & Escalation

**Development Lead:**
- Email: [email]
- Phone: [phone]
- Slack: [handle]

**QA Lead:**
- Email: [email]
- Phone: [phone]
- Slack: [handle]

**Operations Lead:**
- Email: [email]
- Phone: [phone]
- Slack: [handle]

**Emergency Contact:** [number]

---

**Deployment Checklist Version:** 1.0
**Last Updated:** 2024
**Status:** Ready for Deployment ✅
