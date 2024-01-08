/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ["./Areas/**/*.cshtml", "./Pages/**/*.cshtml", "./Pages/**/*.razor", "./wwwroot/js/**/*.js"],
    theme: {
        extend: {},
    },
    safelist: [
        'text-gray-500',
        'text-purple-500',
        'text-blue-500',
        'text-green-500',
        'text-yellow-500',
        'text-orange-500',
        'text-red-500',
        'bg-gray-100',
        'bg-purple-100',
        'bg-blue-100',
        'bg-green-100',
        'bg-yellow-100',
        'bg-orange-100',
        'bg-red-100',
        'border-gray-200',
        'border-purple-200',
        'border-blue-200',
        'border-green-200',
        'border-yellow-200',
        'border-orange-200',
        'border-red-200',
    ],
    plugins: [
        require("daisyui"),
    ],
    daisyui: {
        themes: [
            {
                default: {
                    "primary": "#6b21a8",
                    "secondary": "#fbbf24",
                    "accent": "#3730a3",
                    "neutral": "#404040",
                    "base-100": "#fafafa",
                },
            },
        ],
        darkTheme: "dark",
        base: true,
        styled: true,
    },
}
