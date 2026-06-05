# SomaShare - Missing Features Implementation Guide

## Overview
This document outlines the implementation of missing features for the SomaShare application:
1. Chat/Messaging system views
2. Language toggle functionality
3. User profile integration across listings
4. Enhanced UI for better user experience

---

## 1. Chat/Messaging System

### Files Created:

#### Controllers:
- **SomaShare/Controllers/ChatController.cs** - Handles messaging operations
  - `Index()` - Display list of conversations
  - `Conversation(userId)` - View individual conversation with a user
  - `SendMessage(toUserId, message)` - Send a message
  - `StartConversation(userId)` - Initiate conversation with a user

#### Views:
- **SomaShare/Views/Chat/Index.cshtml** - List all conversations
- **SomaShare/Views/Chat/Conversation.cshtml** - Individual conversation view with:
  - Message history (ordered chronologically)
  - Message input form
  - Auto-refresh every 3 seconds
  - Sender profile information displayed

### Key Features:
- Displays list of active conversations
- Shows last message preview
- Auto-refreshing conversation view (real-time feel)
- Display of other user's profile info (name, institution, course, rating)
- "Send Message" button functionality

### Usage Routes:
```
GET  /Chat/Index - View all conversations
GET  /Chat/Conversation/{userId} - View conversation with specific user
POST /Chat/SendMessage - Submit a message
GET  /Chat/StartConversation/{userId} - Initiate conversation
```

---

## 2. Language Toggle System

### Files Created:

#### Controllers:
- **SomaShare/Controllers/LanguageController.cs** - Handles language switching
  - `SetLanguage(language, returnUrl)` - Change language and redirect

#### Views:
- **SomaShare/Views/Shared/_LanguageToggle.cshtml** - Standalone language toggle partial
- **SomaShare/Views/Shared/_Navbar.cshtml** - Navbar with integrated language dropdown

### Supported Languages:
- 🇬🇧 English (en)
- 🇫🇷 Français (fr)
- 🇿🇦 Zulu (zu)

### Implementation:
- Language preference stored in browser cookie (expires in 1 year)
- Language dropdown in navbar with flag emojis
- Maintains current page URL on language switch

### Usage Route:
```
GET /Language/SetLanguage?language={en|fr|zu}&returnUrl={currentUrl}
```

---

## 3. User Profile System

### Files Created:

#### Controllers:
- **SomaShare/Controllers/ProfileController.cs** - User profile management
  - `View(userId)` - Display any user's public profile
  - `MyProfile()` - Redirect to current user's profile

#### Views:
- **SomaShare/Views/Profile/View.cshtml** - Comprehensive user profile page showing:
  - User avatar/profile picture
  - User rating and reviews count
  - Active textbook listings
  - Contact options (message, view profile)
  - Edit profile button (if viewing own profile)

### Profile Information Displayed:
- Full Name
- Institution
- Course
- Campus
- Email
- User Rating (with stars)
- Number of Active Listings
- Number of Reviews
- Profile Image
- Verification badges

### Usage Routes:
```
GET /Profile/View/{userId} - View any user's profile
GET /Profile/MyProfile - View current user's profile (redirects to /Profile/View/currentUserId)
```

---

## 4. User Profile Integration in Listings

### Enhanced Views Created:

#### Textbook Listings:
- **SomaShare/Views/Textbook/Index_Enhanced.cshtml** - Textbook marketplace with seller cards
  - Each listing displays seller information
  - Clickable seller profile links
  - "Contact Seller" button (direct chat link)
  - Seller rating and institution display
  - Search, filter, and sort functionality

- **SomaShare/Views/Textbook/Details_Enhanced.cshtml** - Detailed textbook view
  - Large seller profile card
  - Seller contact options
  - Seller's other listings link
  - Offer history with offeror information

#### Wanted Ads:
- **SomaShare/Views/WantedAd/Index_Enhanced.cshtml** - Wanted ads marketplace
  - Each ad shows requester information
  - Requester profile links
  - "Contact Requester" button
  - Requester rating and institution
  - Posted date information

#### Offers:
- **SomaShare/Views/Offer/Index_Enhanced.cshtml** - Browse all offers
  - Shows offeror information
  - Price comparison (listed vs. offered)
  - Message offeror functionality
  - Status badges (Pending/Accepted/Declined)

- **SomaShare/Views/Offer/Details_Enhanced.cshtml** - Offer details page
  - Textbook information with pricing
  - Offeror profile card with contact option
  - Seller profile card (for offeror view)
  - Accept/Decline buttons (for sellers)
  - Discount percentage calculation

### Shared Components:
- **SomaShare/Views/Shared/_UserCard.cshtml** - Reusable user profile card component
  - Profile picture
  - User name and rating
  - Institution and course
  - Profile link and message button

### Profile Display Features:
- **Profile Images**: Circular avatars with fallback icon
- **Rating Badges**: Star ratings with visual badge styling
- **Quick Stats**: Institution, course, campus information
- **Action Buttons**: Profile links and direct messaging
- **Hover Effects**: Enhanced card styling on interaction

---

## 5. Navigation & Layout

### Files Created:
- **SomaShare/Views/Shared/_Navbar.cshtml** - Enhanced navbar with:
  - Navigation links to main sections
  - Language toggle dropdown
  - User menu with logout
  - Responsive mobile menu
  - Login/Register links for non-authenticated users

### Navbar Features:
- **Main Navigation**:
  - 📚 Textbooks
  - 📣 Wanted Ads
  - 💬 Forum
  - ✉️ Messages (authenticated users only)
  - 📊 Dashboard (authenticated users only)

- **Language Selector**: Dropdown with flag emojis
- **User Menu**: Profile, edit profile, logout options
- **Responsive**: Mobile-friendly hamburger menu

---

## 6. Database Models (No Changes Required)

All necessary models already exist:
- `ApplicationUser` - Extended with Profile Image URL and Rating
- `ChatMessage` - For messages
- `Textbook`, `WantedAd`, `Offer`, `Transaction`, `Review` - Already present
- `ForumThread`, `ForumPost` - Forum functionality

---

## 7. Integration Steps

### Step 1: Update Existing Views
Replace existing view files with enhanced versions:
- Copy content from `Views/Textbook/Index_Enhanced.cshtml` → `Views/Textbook/Index.cshtml`
- Copy content from `Views/Textbook/Details_Enhanced.cshtml` → `Views/Textbook/Details.cshtml`
- Copy content from `Views/WantedAd/Index_Enhanced.cshtml` → `Views/WantedAd/Index.cshtml`
- Copy content from `Views/Offer/Index_Enhanced.cshtml` → `Views/Offer/Index.cshtml`
- Copy content from `Views/Offer/Details_Enhanced.cshtml` → `Views/Offer/Details.cshtml`

### Step 2: Update Layout
Add navbar partial to `_Layout.cshtml`:
```html
@await Html.PartialAsync("_Navbar")
```

### Step 3: Verify Controllers
Ensure controllers handle necessary includes:
```csharp
.Include(x => x.User)
.Include(x => x.User.Reviews)
```

### Step 4: Add Bootstrap & Font Awesome
Ensure these are in `_Layout.cshtml`:
```html
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" />
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
```

---

## 8. User Workflows

### Sending a Message:
1. User navigates to a textbook/ad/offer listing
2. Clicks "Contact Seller/Requester" button
3. Redirected to `/Chat/Conversation/{userId}`
4. Conversation history loads
5. User types message and hits "Send"
6. Page reloads showing new message

### Viewing User Profile:
1. Click on user name or profile link anywhere in app
2. Navigated to `/Profile/View/{userId}`
3. Displays user's public profile information
4. Option to send message or view their listings

### Switching Language:
1. Click "Language" dropdown in navbar
2. Select desired language (English/Français/Zulu)
3. Page redirects to same URL with language cookie set
4. Content language updates on next page load

### Browsing Listings with User Info:
1. Visit textbooks/wanted ads/offers page
2. Each listing card shows seller/requester info
3. Can click seller name to view their profile
4. Can click "Contact" to open messaging

---

## 9. Responsive Design

All views are responsive with:
- Mobile-first Bootstrap grid system
- Hamburger menu on small screens
- Touch-friendly buttons and links
- Optimized image sizing
- Readable text on all devices

---

## 10. Future Enhancements

Potential additions:
1. **Real-time Chat**: Implement SignalR for WebSocket-based messaging
2. **Message Notifications**: Toast notifications for new messages
3. **Typing Indicators**: Show when other user is typing
4. **Message Read Status**: Show if message has been read
5. **Conversation Search**: Search across message history
6. **Message Attachments**: Share images or files
7. **Localization**: Implement i18n for translated content
8. **User Blocking**: Option to block/report users
9. **Message Pinning**: Pin important messages
10. **Automated Translations**: Real-time message translation

---

## 11. Files Summary

### New Controllers Created:
- ChatController.cs
- ProfileController.cs
- LanguageController.cs

### New Views Created:
- Chat/Index.cshtml
- Chat/Conversation.cshtml
- Profile/View.cshtml
- Textbook/Index_Enhanced.cshtml
- Textbook/Details_Enhanced.cshtml
- WantedAd/Index_Enhanced.cshtml
- Offer/Index_Enhanced.cshtml
- Offer/Details_Enhanced.cshtml
- Shared/_Navbar.cshtml
- Shared/_LanguageToggle.cshtml
- Shared/_UserCard.cshtml

**Total: 3 Controllers + 12 Views = 15 New Files**

---

## 12. Testing Checklist

- [ ] Chat messages send and display correctly
- [ ] Messages auto-refresh every 3-5 seconds
- [ ] User profiles load with correct information
- [ ] Profile images display or show placeholder
- [ ] Language toggle changes cookie and maintains URL
- [ ] User profile links navigate to correct profile
- [ ] "Contact User" buttons redirect to chat
- [ ] All listings show seller/requester information
- [ ] Rating badges display correctly
- [ ] Responsive design works on mobile/tablet/desktop
- [ ] All navigation links function properly
- [ ] Dropdown menus work on mobile
- [ ] Login/logout functionality intact

---

## Installation & Deployment

1. Copy all new controller files to `Controllers/` directory
2. Copy all new view files to corresponding `Views/` directories
3. Update existing view files with enhanced content
4. Update `_Layout.cshtml` to include navbar partial
5. Ensure Bootstrap 5+ and Font Awesome 6+ are included
6. Run `dotnet ef database update` (migration already applied)
7. Test all functionality
8. Deploy to production

---

**End of Documentation**
