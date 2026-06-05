# SomaShare - Quick Start Implementation

## What's Been Added

### ✅ Chat/Messaging Views
- `Views/Chat/Index.cshtml` - List of conversations
- `Views/Chat/Conversation.cshtml` - Individual chat window
- Auto-refreshing messages every 3 seconds

### ✅ Language Toggle Button
- Language selector in navbar (English, Français, Zulu)
- Dropdown menu with flag emojis
- Language preference saved in cookie

### ✅ User Profile System
- Public profile pages for all users
- Profile card with rating, institution, course
- "Send Message" and "View Profile" buttons integrated everywhere

### ✅ Enhanced Listings
- User seller/requester info on every listing
- Clickable seller names linking to profiles
- "Contact Seller/Requester" buttons
- User ratings and institutions displayed

---

## Quick Integration (5 Steps)

### Step 1: Update Controllers Directory
All controller files are already created:
- ✅ ChatController.cs (NEW)
- ✅ ProfileController.cs (NEW)
- ✅ LanguageController.cs (NEW)

### Step 2: Integrate Views

**For Chat Views:**
```
Copy SomaShare/Views/Chat/Index.cshtml
Copy SomaShare/Views/Chat/Conversation.cshtml
```

**For Profile Views:**
```
Copy SomaShare/Views/Profile/View.cshtml
```

**For Enhanced Listings:**
```
Option A: Replace existing with enhanced versions
- Copy content from Index_Enhanced → existing Index.cshtml files
- Copy content from Details_Enhanced → existing Details.cshtml files

Option B: Keep both versions
- Enhanced files demonstrate new features
- Reference them for updates
```

### Step 3: Update _Layout.cshtml

Add navbar before `@RenderBody()`:
```html
@await Html.PartialAsync("_Navbar")

@RenderBody()
```

### Step 4: Verify Bootstrap & Font Awesome

Ensure `_Layout.cshtml` has:
```html
<link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet" />
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
```

### Step 5: Test

Run the application:
```bash
dotnet run
```

Test URLs:
- `https://localhost:7163/Chat/Index` - Messages
- `https://localhost:7163/Profile/View/{userId}` - User profiles
- `https://localhost:7163/Language/SetLanguage?language=fr` - Language toggle

---

## File Structure

```
SomaShare/
├── Controllers/
│   ├── ChatController.cs (NEW)
│   ├── ProfileController.cs (NEW)
│   ├── LanguageController.cs (NEW)
│   └── [other controllers]
├── Views/
│   ├── Chat/
│   │   ├── Index.cshtml (NEW)
│   │   └── Conversation.cshtml (NEW)
│   ├── Profile/
│   │   └── View.cshtml (NEW)
│   ├── Textbook/
│   │   ├── Index_Enhanced.cshtml (NEW - reference/replacement)
│   │   └── Details_Enhanced.cshtml (NEW - reference/replacement)
│   ├── WantedAd/
│   │   └── Index_Enhanced.cshtml (NEW - reference/replacement)
│   ├── Offer/
│   │   ├── Index_Enhanced.cshtml (NEW - reference/replacement)
│   │   └── Details_Enhanced.cshtml (NEW - reference/replacement)
│   └── Shared/
│       ├── _Navbar.cshtml (NEW)
│       ├── _LanguageToggle.cshtml (NEW - optional)
│       └── _UserCard.cshtml (NEW - reusable component)
└── IMPLEMENTATION_GUIDE.md (Documentation)
```

---

## Key Features Overview

### 🔐 Chat System
- Send messages between users
- View conversation history
- Display sender profile info
- Auto-refresh (3-5 second intervals)
- User status and ratings visible

### 🌍 Language Toggle
- 3 language options (English, French, Zulu)
- Navbar dropdown selector
- Cookie-based preference persistence
- Maintains current page on language change

### 👥 User Profiles
- Public profile pages
- User stats (listings, reviews, rating)
- Contact/message buttons
- Profile image or placeholder avatar
- Institution and course information

### 📚 Integrated Listings
- Seller/requester info on cards
- Quick profile links
- Direct contact buttons
- User ratings visible
- Responsive cards with hover effects

---

## Routes Reference

### Chat Routes
```
GET  /Chat/Index                              → All conversations
GET  /Chat/Conversation/{userId}              → Chat with specific user
GET  /Chat/StartConversation/{userId}         → Initiate chat
POST /Chat/SendMessage                        → Send message
```

### Profile Routes
```
GET /Profile/View/{userId}                    → View any user's profile
GET /Profile/MyProfile                        → View own profile
```

### Language Routes
```
GET /Language/SetLanguage?language={code}&returnUrl={url}
```

---

## Usage Examples

### Access Chat
1. Navigate to any listing or user profile
2. Click "Send Message" or "Contact User"
3. Redirected to `/Chat/Conversation/{userId}`
4. View full message history
5. Type and send messages

### View User Profile
1. Click on any user's name or avatar
2. Navigated to `/Profile/View/{userId}`
3. See user's statistics and information
4. Option to send message or view listings

### Change Language
1. Click "Language" button in navbar
2. Select desired language (🇬🇧 English / 🇫🇷 Français / 🇿🇦 Zulu)
3. Page reloads with new language
4. Preference saved to browser cookie

---

## Database Notes

✅ **No migrations needed!**
- All necessary tables already created
- Migration `20260508172000_AddChatMessageAndForumThreadUserRelationships` already applied
- ChatMessage table exists and ready to use

---

## Styling & Responsiveness

All views include:
- Bootstrap 5 grid system
- Responsive images and cards
- Mobile-friendly buttons
- Touch-optimized spacing
- Hover effects and transitions
- Font Awesome icons

Breakpoints:
- `col-md-*`: Medium screens (768px+)
- `col-lg-*`: Large screens (992px+)
- Default: Mobile-first responsive

---

## Troubleshooting

### Messages not displaying?
- Check browser console for errors
- Verify ChatMessage table exists: `SELECT * FROM ChatMessages`
- Confirm user is authenticated

### Profile pictures not showing?
- Check ProfileImageUrl field has correct URL
- Verify image path is accessible
- Placeholder icon displays if URL is null

### Language toggle not working?
- Clear browser cookies: `domain: localhost`
- Check LanguageController is registered in routes
- Verify language parameter is valid (en, fr, zu)

### Users not linking?
- Verify UserId is populated in all tables
- Check Include() statements in controllers
- Confirm User navigation properties exist

---

## Next Steps

1. ✅ Review all created files
2. ✅ Update existing views with enhanced content
3. ✅ Test Chat, Profile, and Language features
4. ✅ Customize styling as needed
5. ✅ Deploy to production

---

## Support & Customization

### To customize colors:
- Edit card styling in view files
- Change `btn-primary`, `bg-success`, etc. to desired Bootstrap classes
- Modify hover effects in `<style>` sections

### To customize languages:
- Add more languages in `_Navbar.cshtml`
- Update Language dropdown options
- Implement actual translations (i18n library)

### To improve performance:
- Implement SignalR for real-time chat (instead of polling)
- Add caching for user profiles
- Implement pagination for large lists

---

**Implementation Complete! 🎉**

All files are ready to use. Follow the Quick Integration steps above to get started.
