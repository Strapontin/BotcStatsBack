import { Role } from "@/entities/Role";
import { Container, Input, Loading, Spacer } from "@nextui-org/react";
import { Fragment, useState } from "react";
import { removeDiaLowerCase } from "../helper/string";
import Classes from "./AutocompleteAddRole.module.css";
import { X } from "react-feather";
import ListItemRole from "../list-stats/ListItemRole";

export default function AutocompleteAddRole(props: { roles: Role[] }) {
  const allRoles = props.roles;
  const [isVisible, setIsVisible] = useState(false);
  const [roleSelected, setRoleSelected] = useState("");

  // All roles filtered with the input
  const allVisibleRoles = allRoles.filter((role) =>
    removeDiaLowerCase(role.name).includes(removeDiaLowerCase(roleSelected))
  );
  const [visibleRoles, setVisibleRoles] = useState<Role[]>(allVisibleRoles);

  if (props.roles.length === 0) return <Loading />;

  // When the user types
  function inputChanged(inputValue: string) {
    setRoleSelected(inputValue);
    setVisibleRoles(allVisibleRoles);

    console.log(inputValue);
    console.log(roleSelected);
  }

  // Hides/Show the list of roles filtered
  function setValuesToSelectVisible(visible: boolean) {
    setIsVisible(visible);
    setVisibleRoles(allVisibleRoles);
  }

  function onSelectRole(id: number) {
    console.log(id);
  }

  return (
    <div className={Classes["autocomplete-role"]}>
      <div className={Classes["input-container"]}>
        <Input
          css={{ flex: 1 }}
          labelPlaceholder="Rôle"
          aria-label="Rôle"
          clearable
          bordered
          // value={roleSelected}
          onChange={(event) => inputChanged(event.target.value)}
          onFocus={() => setValuesToSelectVisible(true)}
          // onClearClick={() => inputChanged("")}
          // onBlur={() => setValuesToSelectVisible(false)}
        ></Input>
        <Spacer x={0.75} />
        <X className={Classes.delete} />
      </div>
      {isVisible && <Spacer y={0.75} />}
      {isVisible && (
        <div className={Classes["container-roles-values"]}>
          {visibleRoles.map((role) => (
            <Fragment key={role.id}>
              <ListItemRole
                image={role.name}
                characterType={role.characterType}
                onClick={() => onSelectRole(role.id)}
              />
              <Spacer y={0.75} />
            </Fragment>
          ))}
        </div>
      )}
    </div>
  );
}
