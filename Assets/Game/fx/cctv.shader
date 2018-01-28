Shader "CCTV"
{
	Properties
    {
        _MainTex ("", 2D) = "white" { }
        _ScanlineColorR ("Scanline Color Red", Color)   = (1, 0.5, 0.8, 1)
        _ScanlineColorG ("Scanline Color Green", Color) = (0.4, 1, 0.8, 1)
        _ScanlineColorB ("Scanline Color Blue", Color)  = (0.2, 0.8, 1, 1)
	}
	SubShader
	{

		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

            uniform sampler2D _MainTex;

            uniform fixed4 _ScanlineColorR;
            uniform fixed4 _ScanlineColorG;
            uniform fixed4 _ScanlineColorB;

            float2 Distort(float2 p)
            {
                float theta = atan2(p.y, p.x);
                float radius = length(p);
                radius = pow(radius, 1.2 /* power */);
                p.x = radius * cos(theta);
                p.y = radius * sin(theta);
                return 0.5 * (p + 1);
            }

			fixed4 frag (v2f i) : SV_Target
			{
                fixed4 scanlineColors[3] = { _ScanlineColorR, _ScanlineColorG, _ScanlineColorB };

                float2 center = 2.0 * i.uv - 1.0;
                float2 uv = i.uv;
                float d = length(center);
                //if (d < 1.0)
                    uv = Distort(center);

				fixed4 color = tex2D(_MainTex, uv);

                color *= scanlineColors[(uint)(i.vertex.y + _Time.y * 15) / 1 % 3];

                //float avg = (color.r + color.g + color.b) / 3;
				//color = fixed4(avg, avg, avg, 1);

                return color;
			}
			ENDCG
		}
	}
}
