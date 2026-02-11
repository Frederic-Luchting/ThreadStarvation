# Thread Starvation Presentation

This folder contains an interactive online presentation about thread starvation, designed for both technical and non-technical audiences.

## 🎯 Purpose

The presentation explains the Thread Starvation demo in business-friendly terms, making it easy to understand:
- What thread starvation is
- Why it matters for business
- The four async patterns and their impact
- Real-world consequences and costs
- How to fix and prevent these issues

## 🚀 How to View

### Option 1: Open Locally
Simply open `index.html` in any modern web browser:
```bash
# From the presentation directory
open index.html         # macOS
xdg-open index.html     # Linux
start index.html        # Windows
```

### Option 2: Local Web Server
For the best experience, serve via HTTP:
```bash
# Using Python 3
python -m http.server 8000

# Using Node.js (with npx)
npx http-server

# Then open: http://localhost:8000
```

### Option 3: GitHub Pages
If this repository has GitHub Pages enabled, the presentation will be available at:
`https://[username].github.io/ThreadStarvation/presentation/`

## 📖 Navigation

- **Arrow Keys**: Navigate between slides (← → ↑ ↓)
- **Space**: Next slide
- **ESC**: Slide overview mode
- **F**: Fullscreen mode

## 📊 Presentation Structure

1. **Introduction** - What is thread starvation?
2. **Business Impact** - Why business should care
3. **Restaurant Analogy** - Easy to understand comparison
4. **Pattern 1: Full-Sync** - Traditional approach
5. **Pattern 2: Async-over-Sync** - Acceptable compromise
6. **Pattern 3: Sync-over-Async** - The dangerous anti-pattern ☠️
7. **Pattern 4: Full-Async** - The optimal solution 🚀
8. **Comparison** - Side-by-side analysis
9. **Visualizations** - Thread pool behavior
10. **Key Takeaways** - Business summary
11. **Investment Perspective** - Cost/benefit analysis
12. **Live Demo** - How to use this repository
13. **Developer Actions** - What to do next
14. **Resources** - Where to learn more

## 🎨 Features

- **Clean, professional design** using Reveal.js framework
- **Color-coded patterns**: Green (good), Orange (warning), Red (danger)
- **Business-friendly language** with technical depth available
- **Keyboard navigation** for smooth presentation flow
- **Slide numbers and progress bar** for easy tracking
- **Overview mode** (press ESC) to see all slides at once
- **Responsive design** works on desktop and tablets

## 🔧 Customization

The presentation uses Reveal.js CDN links, so no installation is needed. To customize:

1. **Theme**: Change the CSS theme link in the HTML
2. **Colors**: Edit the `<style>` section in the HTML
3. **Content**: Edit slide content directly in the HTML
4. **Transitions**: Modify the Reveal.initialize() configuration

## 📝 Technical Details

- **Framework**: Reveal.js 4.5.0
- **Theme**: Black theme (can be changed)
- **Format**: Single HTML file with embedded styles
- **Dependencies**: Loaded via CDN (requires internet connection)
- **Browser Support**: All modern browsers (Chrome, Firefox, Safari, Edge)

## 🎓 Target Audience

This presentation is designed for:
- **Business stakeholders** - Understand costs and risks
- **Product managers** - Make informed technical decisions
- **Non-technical team members** - Learn about performance issues
- **Developers** - Review best practices
- **Anyone interested** - Clear explanations for all levels

## 💡 Tips for Presenters

1. Start with the business impact slides to capture attention
2. Use the restaurant analogy to explain threading concepts
3. Show the live demo for dramatic effect (especially the crash!)
4. Emphasize the ROI of fixing these issues
5. End with clear action items for the team

## 📚 Related Resources

- Main repository README: `../README.md`
- Technical diagrams: `../threading-diagrams.md`
- Demo code: `../Threading/` and `../Requestor/`
- Original talk: [NDC London 2018 on YouTube](https://www.youtube.com/watch?v=RYI0DHoIVaA)
