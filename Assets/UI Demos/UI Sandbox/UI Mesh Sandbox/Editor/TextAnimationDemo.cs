using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using TextElement = UnityEngine.UIElements.TextElement;

public class TextAnimation : EditorWindow
{
    [MenuItem("Window/UI Toolkit/Typewriter Text")]
    public static void ShowExample()
    {
        TextAnimation wnd = GetWindow<TextAnimation>();
        wnd.titleContent = new GUIContent("Typewriter Text");
    }

    Label label;
    private Button _animationButton;
    private FloatField _animationDurationField;
    private float _animationDuration = 5f;
    float elapsed = 0f;
    IVisualElementScheduledItem animationJob;
    bool isTextVisible = true;

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        var container = new VisualElement()
        {
            style =
            {
                flexGrow = 1,
                top = 0,
                bottom = 0,
                right = 0,
                left = 0
            },
            focusable = true // We can only receive key events on a window with a focusable element.
        };
        label = new Label("This is a typewriter animation effect that can have it's timing changed.") { style = { flexGrow = 1, fontSize = 24, unityTextAlign = TextAnchor.MiddleCenter } };
        _animationButton = new Button(OnButtonClicked) { text = "Toggle Text Animation" };
        _animationDurationField = new FloatField("Animation Duration"){ value = _animationDuration, isDelayed = true};
        
        container.Add(label);
        container.Add(_animationDurationField);
        container.Add(_animationButton);
        root.Add(container);

        _animationDurationField.RegisterValueChangedCallback(OnAnimationDurationChanged);
        
        label.PostProcessTextVertices += OnPostProcessTextVertices;

        animationJob = label.schedule.Execute(UpdateTime).Every(1000 / 60); // 60 FPS
        animationJob.Pause(); // Pause the job until the animation starts
    }

    private void OnAnimationDurationChanged(ChangeEvent<float> evt)
    {
        _animationDuration = evt.newValue;
    }

    private void UpdateTime()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= _animationDuration)
        {
            elapsed = _animationDuration; // Cap at max duration
            animationJob.Pause();
        }

        label.MarkDirtyRepaint();
    }
    
    public void OnButtonClicked()
    {
        if (animationJob.isActive)
            return;

        elapsed = 0f;
        animationJob.Resume();
        isTextVisible = !isTextVisible;
    }

    public void OnSpacebarPressed(KeyDownEvent evt)
    {
        if (evt.keyCode != KeyCode.Space || animationJob.isActive)
            return;

        elapsed = 0f;
        animationJob.Resume();
        isTextVisible = !isTextVisible;
    }

    void OnPostProcessTextVertices(TextElement.GlyphsEnumerable glyphs)
    {
        int glyphsToToggle = (int)(elapsed * glyphs.Count / _animationDuration);

        int toggled = 0;
        foreach (TextElement.Glyph glyph in glyphs)
        {
            if (toggled++ >= glyphsToToggle)
                break;

            var verts = glyph.vertices;
            for (int i = 0; i < verts.Length; i++)
            {
                var v = verts[i];
                var tint = v.tint;
                tint.a = isTextVisible ? (byte)255 : (byte)0;
                v.tint = tint;
                verts[i] = v;
            }
        }
    }
}