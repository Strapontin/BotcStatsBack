import { Role } from "@/entities/Role";
import { Fragment, useEffect, useState } from "react";
import Classes from "./RolesSelector.module.css";
import { Button, Input, Spacer } from "@nextui-org/react";
import { X } from "react-feather";
import ListItemRole from "../list-stats/ListItemRole";
import { getAllRoles } from "../../../data/back-api";

export default function RolesSelector(props: { rolesSelected: Role[] }) {
  const [showRoles, setShowRoles] = useState(false);
  const [allRoles, setAllRoles] = useState<Role[]>([]);
  const [visibleRoles, setVisibleRoles] = useState<Role[]>([]);
  const [selectedRoles, setSelectedRoles] = useState<Role[]>([]);

  useEffect(() => {
    async function initRoles() {
      const tempRoles = await getAllRoles();
      setAllRoles(tempRoles);
      setVisibleRoles(tempRoles);
    }
    initRoles();
  }, []);

  function onSelectRole(idRoleSelected: number) {
    const roleSelected = visibleRoles.find((role) => role.id == idRoleSelected);

    if (roleSelected !== undefined) {
      const roles = selectedRoles;
      roles.push(roleSelected);
      setSelectedRoles(roles);

      setVisibleRoles(
        visibleRoles.filter((role) => role.id !== idRoleSelected)
      );
      setShowRoles(false);
    }
  }

  function removeSelectedRole(id: number) {
    const roleSelected = selectedRoles.find((role) => role.id == id);

    if (roleSelected !== undefined) {
      const allSelectedroles = selectedRoles.filter((role) => role.id !== id);
      setSelectedRoles(allSelectedroles);

      setVisibleRoles(
        allRoles.filter((ar) => !allSelectedroles.some((sr) => sr.id === ar.id))
      );
    }
  }

  function blurInput(event: any) {
    // Not selecting a role => hide roles
    if (
      event === undefined ||
      event === null ||
      event.relatedTarget === undefined ||
      event.relatedTarget === null ||
      event.relatedTarget.classList === undefined ||
      event.relatedTarget.classList === null ||
      !event.relatedTarget.classList.contains(Classes["role-item"])
    ) {
      setShowRoles(false);
    }
  }

  return (
    <Fragment>
      <div className={Classes["roles-selected"]}>
        {selectedRoles.map((role) => (
          <Fragment key={role.id}>
            <div className={Classes["role-selected"]}>
              <ListItemRole
                image={role.name}
                characterType={role.characterType}
              />
              <X
                className={Classes.delete}
                onClick={() => removeSelectedRole(role.id)}
              />
            </div>
            <Spacer x={1.25} />
          </Fragment>
        ))}
      </div>
      <div className={Classes["input-container"]}>
        <Input
          css={{ flex: 1 }}
          labelPlaceholder="Rôle"
          aria-label="Rôle"
          clearable
          bordered
          // value={roleSelected}
          // onChange={(event) => inputChanged(event.target.value)}
          onFocus={(event) => setShowRoles(true)}
          // onClearClick={() => inputChanged("")}
          onBlur={(event) => blurInput(event)}
        ></Input>
        <Spacer x={0.75} />
        {/* <X className={Classes.delete} onClick={props.onDelete} /> */}
      </div>
      {showRoles && <Spacer y={0.75} />}
      {showRoles && (
        <div className={Classes["container-roles-values"]}>
          {visibleRoles.map((role) => (
            <Fragment key={role.id}>
              <Button
                className={Classes["role-item"]}
                onPress={() => onSelectRole(role.id)}
              >
                <ListItemRole
                  image={role.name}
                  characterType={role.characterType}
                />
              </Button>
              <Spacer y={0.75} />
            </Fragment>
          ))}
        </div>
      )}
    </Fragment>
  );
}
