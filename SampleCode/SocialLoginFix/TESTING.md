# Testing Guide: Social Login Redirect-Based OAuth Flow

## Prerequisites

### 1. Google OAuth Setup

1. Go to [Google Cloud Console](https://console.cloud.google.com/)
2. Create a new project or select an existing one
3. Enable Google+ API
4. Go to Credentials → Create Credentials → OAuth 2.0 Client ID
5. Application type: Web application
6. Add authorized redirect URIs:
   - `https://localhost:5001/signin-google` (development)
   - `https://yourdomain.com/signin-google` (production)
7. Copy Client ID and Client Secret to `appsettings.json`

### 2. Facebook OAuth Setup

1. Go to [Facebook Developers](https://developers.facebook.com/)
2. Create a new app (Consumer type)
3. Add Facebook Login product
4. Settings → Basic: Copy App ID and App Secret
5. Facebook Login → Settings:
   - Valid OAuth Redirect URIs: `https://localhost:5001/signin-facebook`
6. Copy App ID and App Secret to `appsettings.json`

### 3. Configuration

Update `appsettings.json`:
```json
{
  "Authentication": {
    "Google": {
      "ClientId": "YOUR_ACTUAL_GOOGLE_CLIENT_ID.apps.googleusercontent.com",
      "ClientSecret": "YOUR_ACTUAL_GOOGLE_CLIENT_SECRET"
    },
    "Facebook": {
      "AppId": "YOUR_ACTUAL_FACEBOOK_APP_ID",
      "AppSecret": "YOUR_ACTUAL_FACEBOOK_APP_SECRET"
    }
  }
}
```

## Test Cases

### Test Case 1: Google OAuth with Popup Blocker Enabled

**Objective:** Verify that Google OAuth works with popup blocker enabled

**Steps:**
1. Open Chrome
2. Ensure popup blocker is enabled (default setting)
3. Navigate to `/Account/Login`
4. Click "Sign in with Google" button
5. Observe browser behavior

**Expected Results:**
- ✅ Browser does NOT show "popup blocked" notification
- ✅ Browser redirects to Google's OAuth consent page
- ✅ Page shows Google account selection
- ✅ After selecting account, redirects back to application
- ✅ User is logged in successfully

**Actual Results:**
- [ ] Pass
- [ ] Fail (describe issue): _______________

---

### Test Case 2: Facebook OAuth with Popup Blocker Enabled

**Objective:** Verify that Facebook OAuth works with popup blocker enabled

**Steps:**
1. Open Firefox
2. Ensure popup blocker is enabled (default setting)
3. Navigate to `/Account/Login`
4. Click "Sign in with Facebook" button
5. Observe browser behavior

**Expected Results:**
- ✅ Browser does NOT show "popup blocked" notification
- ✅ Browser redirects to Facebook's OAuth consent page
- ✅ After authentication, redirects back to application
- ✅ User is logged in successfully

**Actual Results:**
- [ ] Pass
- [ ] Fail (describe issue): _______________

---

### Test Case 3: Return URL Preservation (Deep Linking)

**Objective:** Verify that users return to intended page after OAuth

**Steps:**
1. Navigate to a protected page: `/Orders/Details/123`
2. Application redirects to `/Account/Login?returnUrl=%2FOrders%2FDetails%2F123`
3. Click "Sign in with Google"
4. Complete Google OAuth
5. Observe final destination

**Expected Results:**
- ✅ After successful login, user is redirected to `/Orders/Details/123`
- ✅ NOT redirected to home page or account page
- ✅ Return URL is preserved throughout OAuth flow

**Actual Results:**
- [ ] Pass
- [ ] Fail (describe issue): _______________

---

### Test Case 4: OAuth Error Handling - User Cancels

**Objective:** Verify user-friendly error message when user cancels OAuth

**Steps:**
1. Navigate to `/Account/Login`
2. Click "Sign in with Google"
3. On Google consent page, click "Cancel" or "Back to app"
4. Observe application response

**Expected Results:**
- ✅ Redirected back to login page
- ✅ Error message displayed: "Error from external provider: access_denied"
- ✅ Message is user-friendly and visible
- ✅ No application crash or stack trace shown

**Actual Results:**
- [ ] Pass
- [ ] Fail (describe issue): _______________

---

### Test Case 5: OAuth Error Handling - Network Failure

**Objective:** Verify graceful handling of network issues

**Steps:**
1. Navigate to `/Account/Login`
2. Click "Sign in with Google"
3. On Google page, disconnect internet
4. Try to complete authentication
5. Observe application behavior when redirected back

**Expected Results:**
- ✅ Application handles network errors gracefully
- ✅ User-friendly error message displayed
- ✅ User can retry authentication
- ✅ No sensitive error information exposed

**Actual Results:**
- [ ] Pass
- [ ] Fail (describe issue): _______________

---

### Test Case 6: Mobile - iOS Safari

**Objective:** Verify OAuth works on iOS Safari

**Steps:**
1. Open Safari on iPhone/iPad
2. Navigate to application login page
3. Click "Sign in with Google"
4. Complete authentication
5. Verify redirect back to app

**Expected Results:**
- ✅ No popup blocker issues
- ✅ Smooth transition to Google OAuth
- ✅ Smooth transition back to app
- ✅ User is logged in successfully
- ✅ UI is responsive and usable

**Actual Results:**
- [ ] Pass
- [ ] Fail (describe issue): _______________

---

### Test Case 7: Mobile - Chrome Android

**Objective:** Verify OAuth works on Android Chrome

**Steps:**
1. Open Chrome on Android device
2. Navigate to application login page
3. Click "Sign in with Facebook"
4. Complete authentication
5. Verify redirect back to app

**Expected Results:**
- ✅ No popup blocker issues
- ✅ Smooth transition to Facebook OAuth
- ✅ Smooth transition back to app
- ✅ User is logged in successfully
- ✅ UI is responsive and usable

**Actual Results:**
- [ ] Pass
- [ ] Fail (describe issue): _______________

---

### Test Case 8: Multiple Providers

**Objective:** Verify user can switch between providers

**Steps:**
1. Login with Google
2. Logout
3. Login with Facebook
4. Verify both work correctly

**Expected Results:**
- ✅ Both Google and Facebook OAuth work
- ✅ User can use either provider
- ✅ Logout works correctly
- ✅ No conflicts between providers

**Actual Results:**
- [ ] Pass
- [ ] Fail (describe issue): _______________

---

### Test Case 9: CSRF Protection

**Objective:** Verify anti-forgery token protection

**Steps:**
1. Open browser developer tools
2. Navigate to `/Account/Login`
3. Inspect "Sign in with Google" form
4. Verify anti-forgery token is present
5. Try to submit form without token
6. Observe response

**Expected Results:**
- ✅ Anti-forgery token is present in form
- ✅ Form submission without valid token is rejected
- ✅ 400 Bad Request or similar error
- ✅ CSRF attack is prevented

**Actual Results:**
- [ ] Pass
- [ ] Fail (describe issue): _______________

---

### Test Case 10: Accessibility

**Objective:** Verify OAuth flow is accessible

**Steps:**
1. Use screen reader (NVDA/JAWS/VoiceOver)
2. Navigate to login page using keyboard only
3. Tab to "Sign in with Google" button
4. Press Enter to activate
5. Complete OAuth flow using keyboard only

**Expected Results:**
- ✅ All elements are keyboard accessible
- ✅ Screen reader announces buttons correctly
- ✅ Focus management works properly
- ✅ OAuth flow can be completed without mouse
- ✅ WCAG 2.1 Level AA compliance

**Actual Results:**
- [ ] Pass
- [ ] Fail (describe issue): _______________

---

## Browser Compatibility Test Matrix

Test all scenarios in the following browsers:

| Browser | Version | Popup Blocker | OAuth Works | Return URL Works | Error Handling |
|---------|---------|---------------|-------------|------------------|----------------|
| Chrome | Latest | Enabled | ☐ | ☐ | ☐ |
| Firefox | Latest | Enabled | ☐ | ☐ | ☐ |
| Safari | Latest | Enabled | ☐ | ☐ | ☐ |
| Edge | Latest | Enabled | ☐ | ☐ | ☐ |
| iOS Safari | Latest | Default | ☐ | ☐ | ☐ |
| Chrome Android | Latest | Default | ☐ | ☐ | ☐ |

## Performance Testing

### Metrics to Track

1. **Time to Authentication:**
   - Start: User clicks "Sign in with Google"
   - End: User is redirected back and logged in
   - Target: < 3 seconds (excluding provider auth time)

2. **Redirect Count:**
   - Should be exactly 2 redirects:
     1. App → OAuth Provider
     2. OAuth Provider → App
   - No extra redirects

3. **Error Recovery Time:**
   - Time from error to user being able to retry
   - Target: Immediate (no page reload needed)

## Security Testing

### Security Checklist

- [ ] HTTPS enforced for all OAuth redirects
- [ ] Anti-forgery tokens validated on form submission
- [ ] Return URL validated to prevent open redirects
- [ ] OAuth state parameter used and validated
- [ ] No sensitive data in URL parameters
- [ ] Tokens stored securely (not in localStorage)
- [ ] Session cookies have HttpOnly flag
- [ ] Session cookies have Secure flag
- [ ] Session cookies have SameSite attribute

### Penetration Testing

1. **Open Redirect Test:**
   - Try: `/Account/Login?returnUrl=https://evil.com`
   - Expected: Redirect to home page, NOT evil.com

2. **CSRF Test:**
   - Try to submit login form from different origin
   - Expected: Request rejected due to anti-forgery token

3. **OAuth State Tampering:**
   - Intercept OAuth callback
   - Modify state parameter
   - Expected: Authentication fails

## Acceptance Criteria Verification

- [ ] ✅ Implement redirect-based OAuth flow instead of popup
  - Verified by: Test Cases 1, 2
  
- [ ] ✅ Preserve return URL after OAuth completion
  - Verified by: Test Case 3

- [ ] ✅ Update both Google and Facebook login flows
  - Verified by: Test Cases 1, 2, 8

- [ ] ✅ Add user-friendly error message if OAuth fails
  - Verified by: Test Cases 4, 5

## Test with Popup Blocker Enabled

**How to verify popup blocker is enabled:**

**Chrome:**
1. Settings → Privacy and security → Site settings
2. Pop-ups and redirects
3. Should be "Don't allow sites to send pop-ups or use redirects"

**Firefox:**
1. Settings → Privacy & Security
2. Permissions → Block pop-up windows (checked)

**Safari:**
1. Preferences → Websites → Pop-up Windows
2. When visiting other websites: Block and Notify

**Expected:** OAuth still works perfectly with these settings!

## Reporting Template

```markdown
## Test Execution Report

**Date:** _______________
**Tester:** _______________
**Environment:** _______________

### Summary
- Total Test Cases: 10
- Passed: ___
- Failed: ___
- Blocked: ___

### Failed Tests
1. Test Case #___ : _______________
   - Issue: _______________
   - Severity: _______________
   - Screenshot: _______________

### Browser Compatibility
- Chrome: ✅/❌
- Firefox: ✅/❌
- Safari: ✅/❌
- Edge: ✅/❌
- iOS Safari: ✅/❌
- Chrome Android: ✅/❌

### Performance Metrics
- Average auth time: ___ seconds
- Redirect count: ___
- Error recovery: ___ seconds

### Security Verification
- HTTPS enforced: ✅/❌
- CSRF protection: ✅/❌
- Open redirect prevented: ✅/❌
- Secure cookies: ✅/❌

### Acceptance Criteria
- Redirect-based flow: ✅/❌
- Return URL preserved: ✅/❌
- Google & Facebook updated: ✅/❌
- Error messages: ✅/❌

### Recommendation
☐ Ready for production
☐ Needs fixes (see failed tests)
☐ Requires additional testing

**Notes:** _______________
```

## Automation Recommendations

Consider automating these tests using:
- **Selenium WebDriver** for browser automation
- **Playwright** for modern browser testing
- **Cypress** for E2E testing
- **Postman/Newman** for API testing

Example Selenium test:
```csharp
[Test]
public void GoogleOAuth_WithPopupBlocker_ShouldWork()
{
    // Arrange
    driver.Navigate().GoToUrl(baseUrl + "/Account/Login");
    
    // Act
    var googleButton = driver.FindElement(By.CssSelector("button[value='Google']"));
    googleButton.Click();
    
    // Assert - Should redirect to Google (not show popup blocked)
    Assert.That(driver.Url, Does.Contain("accounts.google.com"));
    Assert.That(driver.FindElement(By.TagName("body")).Text, 
        Does.Not.Contain("popup blocked"));
}
```
