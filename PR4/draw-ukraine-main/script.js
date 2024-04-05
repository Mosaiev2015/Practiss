const createSvg = () => {
	let boundaries;
	var svgNS = "http://www.w3.org/2000/svg";
	var svg = document.createElementNS(svgNS, "svg");

	const height = 500;
	const width = 1000;

	svg.setAttribute("width", width);
	svg.setAttribute("height", height);
	document.body.appendChild(svg);

	fetch("cities.json")
		.then((res) => res.json())
		.then((res) => {
			const gamma = 50;
			const rad = 4;
			const city = res.cities;

			let min_setX = Infinity;
			let min_setY = Infinity;

			city.forEach((city) => {
				const x = city.coordinates[0];
				const y = city.coordinates[1];

				if (min_setX > x) min_setX = x;
				if (min_setY > y) min_setY = y;
			});

			city.forEach((city) => {
				const x = city.coordinates[0];
				const y = city.coordinates[1];
				const text_setX = 5;
				const text_setY = 5;
				const point = document.createElementNS(svgNS, "circle");
				const text = document.createElementNS(svgNS, "text");

				point.setAttribute("cx", 13 + (x - min_setX) * gamma);
				point.setAttribute("cy",height - 22 - (y - min_setY) * gamma);
				point.setAttribute("stroke", "yellow");
				point.setAttribute("fill", "blue");
				point.setAttribute("stroke-width", 5);
				

				point.setAttribute("r", rad);

				text.setAttribute(
					"x",
					13 + text_setX + (x - min_setX) * gamma
				);
				text.setAttribute(
					"y",
					height - 22 + text_setY - (y - min_setY) * gamma
				);
				text.setAttribute("id", city.name);
				text.textContent = city.name;
				text.classList.add("animatable");
				text.classList.add("hidden");

				point.addEventListener("mouseenter", () => {
					const label = document.getElementById(city.name);
					label.classList.remove("hidden");
				});

				point.addEventListener("mouseleave", () => {
					const label = document.getElementById(city.name);
					label.classList.add("hidden");
				});

				svg.appendChild(text);
				svg.appendChild(point);
			});
		});

	fetch("world-administrative-boundaries.json")
		.then((res) => res.json())
		.then((res) => {
			boundaries = res;

			const coordinates = boundaries[0].geo_shape.geometry.coordinates[0];

			const scale = 50;
			let min_setX = Infinity;
			let min_setY = Infinity;

			coordinates.forEach(([x, y]) => {
				if (min_setX > x) min_setX = x;
				if (min_setY > y) min_setY = y;
			});

			var path = document.createElementNS(svgNS, "path");

			let path_string = "M ";

			coordinates.forEach(([x, y]) => {
				path_string += `${(x - min_setX) * scale} ${
					height - (y - min_setY) * scale
				} `;
			});

			path_string += "Z";
			path.setAttribute("d", path_string);
			path.setAttribute("stroke", "blue");
			path.setAttribute("fill", "none");
			path.setAttribute("stroke-width", 2);
			svg.appendChild(path);
		});

	
};

document.addEventListener("DOMContentLoaded", createSvg);
