/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{vue,js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        despacho: {
          'rojo': '#800000',      // Rojo oscuro (Acentos, botones principales, notificaciones)
          'oscuro': '#2D3436',    // Gris muy oscuro (Texto principal, headers, fondo de login card)
          'fondo': '#F5F1E1',     // Beige muy claro (Fondo general de la aplicación)
          'tierra': '#BFA799',    // Tono tierra pálido (Tarjetas secundarias, bordes suaves)
          'rosa': '#B58076',      // Rosa viejo (Acentos secundarios, badges de estatus)
        }
      }
    },
  },
  plugins: [],
}