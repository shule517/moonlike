shader_type canvas_item;

// 白昼夢・生死のはざまのような雰囲気
uniform sampler2D screen_texture : hint_screen_texture, filter_linear_mipmap;
uniform float vignette_intensity : hint_range(0.0, 1.0) = 0.4;
uniform float vignette_softness : hint_range(0.0, 1.0) = 0.5;
uniform float desaturation : hint_range(0.0, 1.0) = 0.3;
uniform float bloom_intensity : hint_range(0.0, 1.0) = 0.15;
uniform float aberration : hint_range(0.0, 0.01) = 0.002;
uniform float breathing_speed : hint_range(0.0, 2.0) = 0.5;
uniform float breathing_intensity : hint_range(0.0, 0.2) = 0.05;
uniform vec3 tint_color : source_color = vec3(0.95, 0.92, 1.0);
uniform float time_offset = 0.0;

void fragment() {
    vec2 uv = SCREEN_UV;

    // 呼吸するような脈動効果
    float breath = sin((TIME + time_offset) * breathing_speed) * 0.5 + 0.5;
    float current_vignette = vignette_intensity + breath * breathing_intensity;

    // 色収差（RGB分離）
    float r = texture(screen_texture, uv + vec2(aberration, 0.0)).r;
    float g = texture(screen_texture, uv).g;
    float b = texture(screen_texture, uv - vec2(aberration, 0.0)).b;
    vec3 color = vec3(r, g, b);

    // 簡易ブルーム（ぼかしサンプリング）
    vec3 bloom = vec3(0.0);
    float blur_size = 0.003;
    for (int x = -2; x <= 2; x++) {
        for (int y = -2; y <= 2; y++) {
            vec2 offset = vec2(float(x), float(y)) * blur_size;
            bloom += texture(screen_texture, uv + offset).rgb;
        }
    }
    bloom /= 25.0;
    color = mix(color, color + bloom * bloom_intensity, bloom_intensity);

    // 彩度を下げる（夢のような色あせた感じ）
    float luminance = dot(color, vec3(0.299, 0.587, 0.114));
    color = mix(color, vec3(luminance), desaturation);

    // 色調補正（少し青白い、幽玄な感じ）
    color *= tint_color;

    // ビネット効果（周辺減光）
    vec2 center = uv - 0.5;
    float vignette = 1.0 - dot(center, center) * (current_vignette * 2.0);
    vignette = smoothstep(0.0, vignette_softness, vignette);
    color *= vignette;

    // 少しだけ明るさを持ち上げる（白昼夢感）
    color = pow(color, vec3(0.95));

    COLOR = vec4(color, 1.0);
}
