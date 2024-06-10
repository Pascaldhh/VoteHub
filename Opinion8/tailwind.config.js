/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./Views/**/*.{html,js,cshtml}", "./Areas/**/*.{html,js,cshtml}", "./wwwroot/**/*.{html,js,cshtml}"],
  theme: {
    extend: {
      container: {
        center: true,
        padding: { DEFAULT: "2REM" },
      },
    },
  },
  plugins: [],
};
