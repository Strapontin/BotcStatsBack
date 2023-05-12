import { Role } from "@/entities/Role";
import { Alignment } from "@/entities/enums/alignment";
import { CharacterType } from "@/entities/enums/characterType";

export async function getAllRoles(apiUrl: string) {
  const response = await fetch(`${apiUrl}/Roles`);
  const data = await response.json();
  const roles: Role[] = [];

  for (const key in data) {
    roles.push(data[key]);
  }

  console.log("getAllRoles");
  return roles;
}

export async function getRoleById(apiUrl: string, id: number) {
  if (isNaN(id)) return;

  const response = await fetch(`${apiUrl}/Roles/${id}`);
  const role = await response.json();

  console.log("getRoleById");
  return role;
}

export async function createNewRole(
  apiUrl: string,
  roleName: string,
  characterType: CharacterType,
  alignment: Alignment
): Promise<boolean> {
  const response = await fetch(`${apiUrl}/Roles`, {
    method: "POST",
    mode: "cors",
    cache: "no-cache",
    credentials: "same-origin",
    headers: {
      "Content-Type": "application/json",
    },
    redirect: "follow",
    referrerPolicy: "no-referrer",
    body: JSON.stringify({ roleName, characterType, alignment }),
  });

  console.log("createPlayer");

  if (!response.ok) {
    const res = await response.json();
    console.log(res);
    return false;
  }

  return true;
}
