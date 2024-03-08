const { join } = require("node:path");

const { loadPackageDefinition, credentials } = require("@grpc/grpc-js");
const { loadSync } = require("@grpc/proto-loader");

const protoPath = join(__dirname, "greet.proto");

const packageDefinition = loadSync(protoPath, {
	keepCase: true,
	longs: String,
	enums: String,
	defaults: true,
	oneofs: true,
});

const greetProto = loadPackageDefinition(packageDefinition).greet;

async function main() {
	const client = new greetProto.Greeter(
		"localhost:5219",
		credentials.createInsecure(),
	);

	client.sayHello({ name: "Izak" }, (err, response) => {
		if (err) {
			console.error(err);

			return;
		}

		console.log(response.message);
	});

	client.getUsers({}, (err, response) => {
		if (err) {
			console.error(err);

			return;
		}

		console.log(response.users);
	});

	client.getUserById({ userId: "1" }, (err, response) => {
		if (err) {
			console.error(err);

			return;
		}

		console.log(response.user);
	});

	client.getUserById({ userId: "100" }, (err, response) => {
		if (err) {
			console.error(err.message);

			return;
		}

		console.log(response.user);
	});
}

main();
